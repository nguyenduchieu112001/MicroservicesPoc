<template>
  <div>
    <div v-if="loading" class="loading-overlay">
      <div class="loader"></div>
    </div>
    <h3>Policy details: {{ policy.number }}</h3>
    <div>
      <div class="row">
        <span><strong>From:</strong> {{ formatDate(policy.dateFrom) }}</span>
      </div>
      <div class="row">
        <span><strong>To:</strong> {{ formatDate(policy.dateTo) }}</span>
      </div>
      <div class="row">
        <span><strong>Policy holder:</strong> {{ policy.policyHolder }}</span>
      </div>
      <div class="row">
        <span><strong>Total premium:</strong> {{ policy.totalPremium }} EUR</span>
      </div>
      <div class="row">
        <span><strong>Product code:</strong> {{ policy.productCode }}</span>
      </div>
      <div class="row">
        <span><strong>Account number:</strong> {{ policyAccountNumber || "Not have account" }}</span>
      </div>
      <div class="row">
        <span><strong>Covers:</strong> {{ policy.covers | join }}</span>
      </div>
      <!-- <div class="row">
                <button type="submit"
                        class="btn btn-primary"
                        v-on:click.stop.prevent="documents">Documents
                </button>
            </div> -->
    </div>
  </div>
</template>

<script>
import { HTTP } from "./http/ApiClient";
import moment from "moment";

export default {
  name: "PolicyDetails",
  props: {
    policyNumber: String,
  },
  data() {
    return {
      policy: {},
      policyAccountNumber: "",
      documentsList: [],
      loading: true,
    };
  },
  created: async function() {
    this.loading = true;

    await new Promise((resolve) => {
      setTimeout(resolve, 5000);
    });

    await this.getPaymentAccount();

    await this.getPolicyDetail();

    this.loading = false;
  },
  filters: {
    join: function(value) {
      if (!value) return "";

      return value.join(", ");
    },
  },
  methods: {
    formatDate(date) {
      return moment(date).format("DD/MM/YYYY");
    },
    async getPaymentAccount() {
      await HTTP.get("Payment/accounts/" + this.policyNumber).then(
        (response) => {
          if (response === undefined) {
            this.policyAccountNumber = "";
          } else {
            this.policyAccountNumber =
              response.data.balance.policyAccountNumber;
          }
        }
      );
    },
    async getPolicyDetail() {
      await HTTP.get("policies/" + this.policyNumber).then((response) => {
        this.policy = response.data.policy;
      });
    },
    // documents: function() {
    //   HTTP.get("documents/" + this.policyNumber).then((response) => {
    //     this.documentsList = response.data.documents;
    //     this.documentsList.forEach((doc) => {
    //       const data = doc.content;
    //       const filename = "Policy-" + this.policyNumber + ".pdf";
    //       const blob = b64toBlob(data, "", 8),
    //         e = document.createEvent("MouseEvents"),
    //         a = document.createElement("a");
    //       a.download = filename;
    //       a.href = window.URL.createObjectURL(blob);
    //       a.dataset.downloadurl = ["text/json", a.download, a.href].join(":");
    //       e.initMouseEvent(
    //         "click",
    //         true,
    //         false,
    //         window,
    //         0,
    //         0,
    //         0,
    //         0,
    //         0,
    //         false,
    //         false,
    //         false,
    //         false,
    //         0,
    //         null
    //       );
    //       a.dispatchEvent(e);
    //     });
    //   });

    //   function b64toBlob(b64Data, contentType, sliceSize) {
    //     contentType = contentType || "";
    //     sliceSize = sliceSize || 512;

    //     const byteCharacters = atob(b64Data);
    //     const byteArrays = [];

    //     for (
    //       let offset = 0;
    //       offset < byteCharacters.length;
    //       offset += sliceSize
    //     ) {
    //       const slice = byteCharacters.slice(offset, offset + sliceSize);

    //       const byteNumbers = new Array(slice.length);
    //       for (let i = 0; i < slice.length; i++) {
    //         byteNumbers[i] = slice.charCodeAt(i);
    //       }

    //       const byteArray = new Uint8Array(byteNumbers);

    //       byteArrays.push(byteArray);
    //     }

    //     return new Blob(byteArrays, { type: contentType });
    //   }
    // },
  },
};
</script>

<style scoped>
.loading-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.8);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;
}
.loader {
  border: 8px solid #f3f3f3; /* Light grey */
  border-top: 8px solid #3498db; /* Blue */
  border-radius: 50%;
  width: 80px;
  height: 80px;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
