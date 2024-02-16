<template>
  <div>
    <div class="filter-container">
      <h4>Search Payment</h4>
      <b-form inline>
        <b-input v-model="filterFields.number" placeholder="Policy number" class="mr-sm-2 mb-sm-0 search-input"/>
        <b-button v-on:click="search()" variant="primary" class="search-button">Search</b-button>
      </b-form>
    </div>

    <b-table bordered striped hover
             :items="payment"
             :fields="fields"
             show-empty
             :empty-text="rowEmpty">
      <template #empty="scope">
        {{ rowEmpty }}
      </template>
    </b-table>
  </div>
</template>

<script>
import { HTTP } from "./http/ApiClient";

export default {
  name: "PaymentList",
  data() {
    return {
      fields: [
        { key: "policyAccountNumber" },
        { key: "policyNumber" },
        { key: "balance" },
      ],
      payment: [],
      filterFields: {
        number: "",
      },
      rowEmpty: "Please input your Policy Number to Search Payment",
    };
  },
  methods: {
    search() {
      if (this.filterFields.number === "") {
        this.payment = [];
      } else {
        HTTP.get("Payment/accounts/" + this.filterFields.number)
          .then((response) => {
            if (response === undefined) {
              this.rowEmpty = `There is no data about policyNumber ${
                this.filterFields.number
              }`;
              this.payment = [];
            } else {
              this.rowEmpty = "";
              this.payment = [response.data.balance];
            }
          })
          .catch((error) => {
            console.error("Error fetching data:", error);
            this.rowEmpty = "Error fetching data. Please try again.";
            this.payment = [];
          });
      }
    },
  },
};
</script>

<style scoped>
.filter-container {
  margin-top: 20px;
  margin-bottom: 20px;
}

.search-input {
  width: 90% !important;
}

.search-button {
  width: 8% !important;
}
</style>
