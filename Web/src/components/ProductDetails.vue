<template>
  <div class="container">
    <div class="row">
      <div class="col-sm-4">
        <img
          class="card-img-top"
          :src="imageSrc"
          :alt="productDetails.name"
        >
      </div>
      <div class="col-sm-8">
        <h1 class="display-3">
          {{ productDetails.name }}
        </h1>
        <p class="lead">
          {{ productDetails.description }}.
        </p>
        <hr class="my-4">
        <p>Enter the data needed to calculate the price</p>
  
        <div class="row">
          <div class="col-sm-12">
            <form>
              <div class="form-group row">
                <label
                  class="col-sm-3 col-form-label"
                  for="policyFrom"
                >Policy from </label>
                <div class="col-sm-9">
                  <b-form-input
                    id="policyFrom"
                    v-model="policyFrom"
                    type="date"
                    name="policyFrom"
                    :disabled="'VIEW' === mode"
                    placeholder="Policy from"
                    :state="isValid(policyFrom) && isPolicyFromValid(policyFrom)"
                  />
                  <b-form-invalid-feedback :state="isValid(policyFrom) && isPolicyFromValid(policyFrom)">
                    Please choose a policyFrom and policyFrom is not earlier than today
                  </b-form-invalid-feedback>
                </div>
              </div>
  
              <div class="form-group row">
                <label
                  class="col-sm-3 col-form-label"
                  for="policyTo"
                >Policy to </label>
                <div class="col-sm-9">
                  <b-form-input
                    id="policyTo"
                    v-model="policyTo"
                    type="date"
                    name="policyTo"
                    :disabled="'VIEW' === mode"
                    placeholder="Policy to"
                    :state="isValid(policyTo) && isPolicyRangeValid()"
                  />
                  <b-form-invalid-feedback :state="isValid(policyTo) && isPolicyRangeValid()">
                    Please choose a policyTo and policyTo is not earlier than policyFrom
                  </b-form-invalid-feedback>
                </div>
              </div>
  
              <div
                v-for="a in answers"
                :key="a.id"
                class="form-group row"
              >
                <label class="col-sm-3 col-form-label">{{ a.question.text }} </label>
  
                <div
                  v-if="a.question.questionType==='Numeric'"
                  class="col-sm-9"
                >
                  <b-form-input
                    v-model="a.answer"
                    required
                    type="number"
                    class="form-control"
                    :disabled="'VIEW' === mode"
                    :state="isValid(a.answer)"
                  />
                  <b-form-invalid-feedback :state="isValid(a.answer)">
                    Please ensure that your answer is not left empty.
                  </b-form-invalid-feedback>
                </div>
  
                <div
                  v-if="a.question.questionType==='Choice'"
                  class="col-sm-9"
                >
                  <b-form-select
                    v-model="a.answer"
                    required
                    :disabled="'VIEW' === mode"
                    :state="isValid(a.answer)"
                  >
                    <option
                      v-for="option in a.question.choices"
                      :key="option.code"
                      :value="option.code"
                    >
                      {{ option.label }}
                    </option>
                  </b-form-select>
                  <b-form-invalid-feedback :state="isValid(a.answer)">
                    Please ensure that your option is chosen.
                  </b-form-invalid-feedback>
                </div>
              </div>
  
              <!-- When haven't created a price calculation formula yet -->
              <div
                v-if="'ERROR' === mode"
                class="form-group row"
              >
                <div class="col-sm-12">
                  <span class="float-left">
                    <strong v-if="user.userTypes.includes('ADMIN')" style="color: red;">Please make sure to create a formula before calculating the price.</strong>
                    <strong v-else style="color: red;">This product does not have a calculation formula for the price. To create a payment formula, please contact your administrator.</strong>
                  </span>
                </div>
              </div>

              <!-- displays price -->
              <div
                v-if="'VIEW' === mode"
                class="form-group row"
              >
                <label class="col-sm-3 col-form-label">Price</label>
                <div class="col-sm-9">
                  <span class="float-left">
                    <strong>{{ price.amountToPay }} EUR</strong>
                  </span>
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  
    <div class="row">
      <div class="col-sm-12 margin-top-10">
        <div
          v-if="'EDIT' === mode"
          class="d-flex flex-row-reverse"
        >
          <div class="p-2">
            <button
              type="submit"
              class="btn btn-primary"
              @click.stop.prevent="calculatePrice"
            >
              Calculate price
            </button>
          </div>
          <div class="p-2">
            <router-link :to="{name: 'products'}">
              <b-button variant="secondary">
                Back
              </b-button>
            </router-link>
          </div>
        </div>
  
        <div
          v-if="'VIEW' === mode"
          class="d-flex flex-row-reverse"
        >
          <div class="p-2">
            <button
              type="submit"
              class="btn btn-primary"
              @click.stop.prevent="createOffer"
            >
              Buy
            </button>
          </div>
          <div class="p-2">
            <a
              class="btn btn-secondary"
              href="#"
              role="button"
              @click.stop.prevent="backToEdit"
            >Change
            parameters</a>
          </div>
          <div class="p-2">
            <router-link :to="{name: 'products'}">
              <b-button variant="secondary">
                Back
              </b-button>
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
  
  <script>
import moment from "moment";
import { HTTP } from "./http/ApiClient";

export default {
  name: "ProductDetails",
  props: {
    productCode: String,
  },
  data() {
    return {
      productDetails: {},
      imageSrc: "",
      answers: [],
      mode: "EDIT",
      price: {
        amountToPay: null,
      },
      policyFrom: "",
      policyTo: "",
      offerNumber: "",
      user: {
        avatar: "",
        login: "",
        userTypes: [],
      },
    };
  },
  created: function() {
    HTTP.get("products/" + this.productCode).then(async (response) => {
      this.productDetails = response.data;
      const responseImage = await HTTP.get("" + this.productDetails.image, {
        responseType: "arraybuffer",
      });

      // Convert the binary data to a data URI
      const arrayBufferView = new Uint8Array(responseImage.data);
      const blob = new Blob([arrayBufferView], { type: "image/jpeg" });
      const imageUrl = URL.createObjectURL(blob);

      this.imageSrc = imageUrl;
      if (!this.productDetails.questions) return;

      for (let i = 0; i < this.productDetails.questions.length; i++) {
        this.answers.push({
          answer: null,
          question: this.productDetails.questions[i],
        });
      }

      await this.getUser();
    });
  },
  methods: {
    async getUser() {
      const API_URL = process.env.VUE_APP_AUTH_URL;
      const LOGIN_URL = API_URL + "User";
      await HTTP.get(LOGIN_URL).then((response) => {
        this.user = response.data;
      });
    },
    backToEdit: function() {
      this.mode = "EDIT";
    },
    isValid(answer) {
      return Boolean(answer);
    },
    isPolicyFromValid() {
      const policyFromDate = moment(this.policyFrom).startOf("day");
      const today = moment().startOf("day");

      return policyFromDate.isSameOrAfter(today);
    },
    isPolicyRangeValid() {
      // Check if policyTo is not earlier than policyFrom
      return new Date(this.policyTo) > new Date(this.policyFrom);
    },
    createRequest: function() {
      const request = {
        productCode: this.productDetails.code,
        policyFrom: this.policyFrom,
        policyTo: this.policyTo,
        selectedCovers: [],
        answers: [],
      };

      for (let i = 0; i < this.productDetails.covers.length; i++) {
        request.selectedCovers.push(this.productDetails.covers[i].code);
      }

      for (let j = 0; j < this.answers.length; j++) {
        request.answers.push({
          questionCode: this.answers[j].question.questionCode,
          questionType: this.answers[j].question.questionType,
          answer: this.answers[j].answer,
        });
      }

      return request;
    },
    areAllAnswersValid() {
      for (let j = 0; j < this.answers.length; j++) {
        const answer = this.answers[j].answer;
        if (!this.isValid(answer)) {
          return false; // At least one answer is invalid
        }
      }
      return true; // All answers are valid
    },
    calculatePrice: function() {
      if (
        !this.isValid(this.policyFrom) ||
        !this.isValid(this.policyTo) ||
        !this.isPolicyRangeValid() ||
        !this.areAllAnswersValid()
      ) {
        setTimeout(() => {
          alert("Please complete the entire form.");
        }, 100);
      } else {
        HTTP.post("offers", this.createRequest(), {
          params: {
            agentLogin: this.user.login,
          },
        }).then((response) => {
          if (response === undefined) {
            this.mode = "ERROR";
          } else {
            this.mode = "VIEW";
            this.price.amountToPay = response.data.totalPrice;
            this.offerNumber = response.data.offerNumber;
          }
        });
      }
    },
    createOffer: function() {
      this.$router.push({
        name: "createPolicy",
        params: {
          offerNumber: this.offerNumber,
        },
        query: {
          productCode: this.productCode,
          policyFrom: this.policyFrom,
          policyTo: this.policyTo,
          totalPremium: this.price.amountToPay,
        },
      });
      // this.$router.push({ name: "policies" });
    },
  },
};
</script>
  
  <style scoped lang="scss">
.margin-top-10 {
  margin-top: 10px;
}
</style>
