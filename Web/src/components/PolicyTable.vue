<template>
  <div>
    <table class="table">
      <thead>
        <tr>
          <th>#</th>
          <th>Policy Number</th>
          <th>Policy Start Date</th>
          <th>Policy End Date</th>
          <th>Policy Holder</th>
          <th>Action</th>
        </tr>
      </thead>
      <tbody>
        <template v-if="policies.length > 0">
          <tr v-for="(policy, index) in policies" :key="policy.policyNumber">
            <th scope="row">{{ index + 1 }}</th>
            <td>{{ policy.policyNumber }}</td>
            <td>{{ policy.policyStartDate }}</td>
            <td>{{ policy.policyEndDate }}</td>
            <td>{{ policy.policyHolder }}</td>
            <td>
              <b-button @click="showDetails(policy.policyNumber)" 
                        variant="info" class="rounded-circle mr-3">
                <i class="bi bi-info-lg"></i>
              </b-button>
              <b-button v-b-modal.DeletePolicy
                        @click="setSelectedPolicy(policy.policyNumber)"
                        variant="danger" class="rounded-circle">
                <i class="bi bi-trash-fill"></i>
              </b-button>
            </td>
          </tr>
        </template>
        <template v-if="policies.length === 0">
          <tr>
            <td :colspan="6">
              <b class="empty-message">{{ rowEmpty }}</b>
            </td>
          </tr>
        </template>
      </tbody>
    </table>
    <b-modal 
      id="DeletePolicy" 
      title="Delete Policy"
      centered 
      @ok="deletePolicy">
      <p class="text-center" style="font-size: 20px; color: red;">
        Do you want to delete this policy with number: {{ selectedPolicy ? selectedPolicy : '' }}?
      </p>
    </b-modal>
  </div>
</template>

<script>
import moment from "moment";
import { HTTP } from "./http/ApiClient";

export default {
  name: "TablePolicy",
  props: {
    policies: {
      type: Array,
      default: () => [],
    },
    rowEmpty: {
      type: String,
      default: "",
    },
  },
  data() {
    return {
      fields: [
        { key: "policyNumber" },
        { key: "policyStartDate" },
        { key: "policyEndDate" },
        { key: "policyHolder" },
        { key: "action" },
      ],
      selectedPolicy: null,
    };
  },
  methods: {
    setSelectedPolicy(policyNumber) {
      this.selectedPolicy = policyNumber;
    },
    showDetails(record) {
      this.$router.push({
        name: "policyDetails",
        params: { policyNumber: record },
      });
    },
    deletePolicy() {
      const request = {
        policyNumber: this.selectedPolicy,
        terminationDate: moment(new Date()).format("YYYY-MM-DD"),
      };
      HTTP.delete("Policy/terminate", {
        data: request,
      });
      this.$bvModal.hide("DeletePolicy");
      this.selectedPolicy = null;
      window.location.reload();
    },
  },
};
</script>
