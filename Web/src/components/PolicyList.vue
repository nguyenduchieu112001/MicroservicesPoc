<template>
  <div>
    <div class="filter-container">
      <h4>Search policies</h4>
      <b-form inline>
        <b-input v-model="filterFields.number" placeholder="Policy number" class="mr-sm-2 mb-sm-0 search-input"/>
        <b-input v-model="filterFields.policyHolder" placeholder="Policy Holder" class="mr-sm-2 mb-sm-0 search-input"/>
        <b-button v-on:click="search()" variant="primary" class="search-button">Search</b-button>
      </b-form>
    </div>

    <PolicyTable
      :policies="policies"
      :rowEmpty="rowEmpty"
    />

  </div>
</template>

<script>
import moment from "moment";
import { HTTP } from "./http/ApiClient";
import PolicyTable from "./PolicyTable.vue";
const AND = "%20AND%20";

export default {
  name: "PolicyList",
  components: { PolicyTable },
  data() {
    return {
      policies: [],
      filterFields: {
        policyHolder: "",
        number: "",
      },
      rowEmpty:
        "Please input your Policy Number or Policy Holder to Search Policies",
    };
  },
  // created: function() {
  //   this.runSearch();
  // },
  methods: {
    search() {
      let queryString = "";
      queryString = this.addCriteria(
        queryString,
        this.filterFields.policyHolder
      );
      queryString = this.addCriteria(queryString, this.filterFields.number);

      this.runSearch(this.formatQueryString(queryString));
    },
    runSearch(queryString = "") {
      if (queryString === "") {
        this.policies = [];
      } else {
        HTTP.get("PolicySearch" + queryString).then((response) => {
          if (response.data.policies.length === 0) {
            this.rowEmpty = `There is no data about policyNumber ${
              this.filterFields.number
            } and policyHolder ${this.filterFields.policyHolder}`;
          } else {
            const policies = response.data.policies.map((policy) => {
              return {
                ...policy,
                policyStartDate: moment(policy.policyStartDate).format(
                  "DD/MM/YYYY"
                ),
                policyEndDate: moment(policy.policyEndDate).format(
                  "DD/MM/YYYY"
                ),
              };
            });
            this.policies = policies;
          }
        });
      }
    },
    addCriteria(queryString, criteria) {
      if (criteria !== "") queryString += criteria + AND;

      return queryString;
    },
    formatQueryString(queryString) {
      if (queryString.endsWith(AND))
        queryString = queryString.substring(0, queryString.length - AND.length);

      return queryString !== "" ? "?q=" + queryString : "";
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
  width: 45% !important;
}

.search-button {
  width: 8% !important;
}
</style>
