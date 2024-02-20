<template>
  <div>
    <b-form
      ref="findCodeForm"
      :model="code"
      @submit.stop.prevent="handleCodeSubmit"
    >
      <b-row>
        <b-col md="8">
          <b-form-group
            label="Code"
            label-for="code"
          >
            <b-form-input
              id="code"
              v-model="code"
              :state="Boolean(code)" 
              required
            />
            <b-form-invalid-feedback :state="Boolean(code)">
              Please ensure that your code is not left empty.
            </b-form-invalid-feedback>
          </b-form-group>
        </b-col>
        <b-col
          md="2"
          class="search_button"
        >
          <b-button
            variant="outline-primary"
            block
            pill
            @click="handleCodeSubmit()"
          >
            <i class="bi bi-plus-lg" />
          </b-button>
        </b-col>
        <b-col
          md="2"
          class="search_button"
        >
          <b-button
            variant="info"
            pill
            block
            @click="handleUpdate()"
          >
            <i class="bi bi-pencil-square" />
          </b-button>
        </b-col>
      </b-row>
    </b-form>
    <br />
    <div class="w-100">
      <b-row>
        <b-col class="text-center">
          <h3>LIST QUESTION CODES</h3>
        </b-col>
      </b-row>
      <b-row class="questionContainer">
        <div
          v-for="(question, index) in questions"
          :key="index"
          class="questionCode"
        >
          <b-col md="3" class="d-flex align-items-center">
            <div class="mr-2">{{ question.questionCode }}</div>
            <div v-if="question.questionType === 'Choice'">
              <i :id="'choices_' + index" class="bi bi-info-circle-fill"></i>
              <b-popover
                :target="'choices_' + index"
                title="Choices"
                triggers="hover focus"
              >
                <ul>
                  <li v-for="(choice, choiceIndex) in question.choices" :key="choiceIndex">
                    {{ choice.label }} - {{ choice.code }}
                  </li>
                </ul>
              </b-popover>
            </div>
          </b-col>
        </div>
      </b-row>
    </div>
    <br />
    <div v-if="questions.length > 0">
      <b-form
        ref="PricingForm"
        :model="CreateCalculatePrice"
        @submit.stop.prevent="handleSubmit"
      >
        <b-tabs
          v-model="activeTab"
          content-class="mt-3"
          fill
        >
          <b-tab title="Base Premium Calculation Rules">
            <b-card>
              <b-row>
                <b-col>
                  <b-button
                    class="float-right rounded-circle"
                    variant="success"
                    @click="addBasePremium()"
                  >
                    <i class="bi bi-plus" />
                  </b-button>
                </b-col>
              </b-row>
              <b-row
                v-for="(basePremiumCalculationRule, index) in CreateCalculatePrice.basePremiumCalculationRules"
                :key="index"
              >
                <b-col md="2">
                  <b-form-group label="Cover Code" label-for="coverCode">
                    <b-form-input
                      id="coverCode"
                      v-model="basePremiumCalculationRule.coverCode"
                      :state="isValidCoverCode(index)"
                      :value="basePremiumCalculationRule.coverCode"
                      required
                    />
                    <b-form-invalid-feedback :state="isValidCoverCode(index)">
                      Invalid Cover Code. Please enter a valid formula like C{number} and not exceeding C{{ num_of_covers }}.
                    </b-form-invalid-feedback>
                  </b-form-group>
                </b-col>
                <b-col md="4">
                  <b-form-group
                    label="Apply If Formula"
                    label-for="applyIfFormula"
                  >
                    <b-form-input
                      id="applyIfFormula"
                      v-model="basePremiumCalculationRule.applyIfFormula"
                      :state="Boolean(isValidApplyIfFormulaInBasePremium(index))"
                      :value="basePremiumCalculationRule.applyIfFormula"
                      aria-describedby="formulaInBasePremium"
                    />
                    <b-form-invalid-feedback :state="Boolean(isValidApplyIfFormulaInBasePremium(index))">
                      Formula must have questionCode
                    </b-form-invalid-feedback>
                    <b-form-text id="formulaInBasePremium">
                      1. Your formula must have questionCode and be a comparison (&lt;, &gt;, &le;, &ge;, ==, !=)
                    </b-form-text>
                    <b-form-text id="formulaInBasePremium">
                      2. If your formula has a questionCode of choice, it must have comparison properties (==, !=) 
                      with the code of choice placed next to the symbol "i" (usually an abbreviation).
                    </b-form-text>
                  </b-form-group>
                </b-col>
                <b-col md="5">
                  <b-form-group
                    label="Base Price Formula"
                    label-for="basePriceFormula"
                  >
                    <b-form-input
                      id="basePriceFormula"
                      v-model="basePremiumCalculationRule.basePriceFormula"
                      :state="Boolean(isValidBasePriceFormula(index))"
                      :value="basePremiumCalculationRule.basePriceFormula"
                      required
                      aria-describedby="BasePremium"
                    />
                    <b-form-invalid-feedback :state="isValidBasePriceFormula(index)">
                      Please ensure that your formula is not left empty.
                    </b-form-invalid-feedback>
                    <b-form-text id="formulaInBasePremium">
                      1. If the Base premium formual is numeric, it must have M at the end of the number (for example, 0.2M).
                    </b-form-text>
                    <b-form-text id="formulaInBasePremium">
                      2. If the base premium formula is a formula, it must contain at least one questionCode of type number
                      (The number code should not be placed next to the symbol "i") and must ensure that the number in the formula satisfies 
                      condition 1 (for example: AREA * 0.2M)
                    </b-form-text>
                  </b-form-group>
                </b-col>
                <b-col
                  md="1"
                  style="display: flex; justify-content: center; align-items: center;"
                >
                  <b-button
                    class="float-right"
                    variant="danger"
                    @click="removeBasePremium(index)"
                  >
                    <i class="bi bi-dash-lg" />
                  </b-button>
                </b-col>
              </b-row>
            </b-card>
          </b-tab>
          <b-tab title="Discount Markup Rules">
            <b-card>
              <b-row>
                <b-col>
                  <b-button
                    class="float-right rounded-circle"
                    variant="success"
                    @click="addDiscount()"
                  >
                    <i class="bi bi-plus" />
                  </b-button>
                </b-col>
              </b-row>
              <b-row
                v-for="(discountMarkupRule, index) in CreateCalculatePrice.discountMarkupRules"
                :key="index"
              >
                <b-col md="8">
                  <b-form-group
                    label="Apply If Formula"
                    label-for="applyIfFormula"
                  >
                    <b-form-input
                      id="applyIfFormula"
                      v-model="discountMarkupRule.applyIfFormula"
                      :state="Boolean(isValidApplyIfFormulaInDiscount(index))"
                      :value="discountMarkupRule.applyIfFormula"
                      required
                      aria-describedby="formulaInDiscount"
                    />
                    <b-form-invalid-feedback :state="Boolean(isValidApplyIfFormulaInDiscount(index))">
                      Formula must have at least 1 questionCode
                    </b-form-invalid-feedback>
                    <b-form-text id="formulaInDiscount">
                      1. Your formula must have questionCode and be a comparison (&lt;, &gt;, &le;, &ge;, ==, !=)
                    </b-form-text>

                    <b-form-text id="formulaInDiscount">
                      2. If your formula has a questionCode of choice, it must have comparison properties (==, !=) 
                      with the code of choice placed next to the symbol "i" (usually an abbreviation).
                    </b-form-text>
                  </b-form-group>
                </b-col>
                <b-col md="2">
                  <b-form-group
                    label="Param Value"
                    label-for="paramValue"
                  >
                    <b-form-input
                      id="paramValue"
                      v-model="discountMarkupRule.paramValue"
                      :state="isValidParamValue(index)"
                      :value="discountMarkupRule.paramValue"
                      required
                    />
                    <b-form-invalid-feedback :state="isValidParamValue(index)">
                      Please ensure that your param value > 0.
                    </b-form-invalid-feedback>
                  </b-form-group>
                </b-col>
                <b-col
                  md="2"
                  style="display: flex; justify-content: center; align-items: center;"
                >
                  <b-button
                    class="float-right"
                    variant="danger"
                    @click="removeDiscount(index)"
                  >
                    <i class="bi bi-dash-lg" />
                  </b-button>
                </b-col>
              </b-row>
            </b-card>
          </b-tab>
        </b-tabs>
        <br />
        <b-button @submit.stop.prevent="handleSubmit" type="submit" variant="primary" class="float-right">Submit</b-button>
      </b-form>
    </div>
  </div>
</template>
  
  <script>
import { HTTP } from "./http/ApiClient";

export const basePremiumCalculationRules = {
  coverCode: "",
  applyIfFormula: "",
  basePriceFormula: "",
};

export const discountMarkupRules = {
  applyIfFormula: "",
  paramValue: 0,
};

export default {
  name: "PricingCreate",
  data() {
    return {
      activeTab: 0,
      code: "",
      covers: [],
      questions: [],
      visible: false,
      num_of_covers: 0,
      CreateCalculatePrice: {
        code: "",
        basePremiumCalculationRules: [basePremiumCalculationRules],
        discountMarkupRules: [discountMarkupRules],
      },
      mode: "",
    };
  },
  methods: {
    async handleCodeSubmit() {
      if (this.code === "") {
        this.covers = [];
        this.questions = [];
        return;
      }
      await HTTP.get("products/" + this.code).then((response) => {
        if (response.data === null) {
          setTimeout(() => {
            alert(`"Not found with code ${this.code}"`);
          }, 100);
        } else {
          this.covers = response.data.covers;
          this.questions = response.data.questions;
          this.num_of_covers = this.covers.length;
          this.CreateCalculatePrice = {
            code: "",
            basePremiumCalculationRules: [basePremiumCalculationRules],
            discountMarkupRules: [discountMarkupRules],
          };
          this.mode = "CREATE";
        }
      });
    },
    isValidCoverCode(index) {
      const coverCodePattern = new RegExp(`^C[1-${this.num_of_covers}]$`);
      const coverCode = this.CreateCalculatePrice.basePremiumCalculationRules[
        index
      ].coverCode;
      return coverCodePattern.test(coverCode);
    },
    isValidApplyIfFormulaInBasePremium(index) {
      const validKeywords = this.questions; // Assuming this is the array of questions
      const expressionToCheck = this.CreateCalculatePrice
        .basePremiumCalculationRules[index].applyIfFormula;
      if (!expressionToCheck || expressionToCheck.trim() === "") {
        return true; // Empty is considered valid
      }

      const numericComparisonRegex = /(?:<|>|<=|>=|==|!=)\s*(-?\d+(\.\d+)?)\s*/;
      const equalityComparisonRegex = /(\w+)\s*==\s*["']([^"']+)["']/;
      const notEqualityComparisonRegex = /(\w+)\s*!=\s*["']([^"']+)["']/;

      const isValid = validKeywords.some((keyword) => {
        if (expressionToCheck.includes(keyword.questionCode)) {
          if (keyword.questionType === "Numeric") {
            return numericComparisonRegex.test(expressionToCheck);
          } else if (keyword.questionType === "Choice") {
            // Check if questionCode in expression matches any choice code
            const hasEquals = equalityComparisonRegex.test(expressionToCheck);
            const hasNotEquals = notEqualityComparisonRegex.test(
              expressionToCheck
            );
            const choices = keyword.choices.map((choice) => choice.code);

            const choicesPattern = new RegExp(`\\b(${choices.join("|")})\\b`);

            return (
              (hasEquals || hasNotEquals) &&
              choicesPattern.test(expressionToCheck)
            );
          }
        }
        return false; // Keyword not found, considered valid
      });

      return isValid;
    },
    isValidApplyIfFormulaInDiscount(index) {
      const validKeywords = this.questions; // Assuming this is the array of questions
      const expressionToCheck = this.CreateCalculatePrice.discountMarkupRules[
        index
      ].applyIfFormula;
      // Check for a numerical comparison in the expression
      const numericComparisonRegex = /(?:<|>|<=|>=|==|!=)\s*(-?\d+(\.\d+)?)\s*/;
      const equalityComparisonRegex = /(\w+)\s*==\s*["']([^"']+)["']/;
      const notEqualityComparisonRegex = /(\w+)\s*!=\s*["']([^"']+)["']/;

      const isValid = validKeywords.some((keyword) => {
        if (expressionToCheck.includes(keyword.questionCode)) {
          if (keyword.questionType === "Numeric") {
            return numericComparisonRegex.test(expressionToCheck);
          } else if (keyword.questionType === "Choice") {
            // Check if questionCode in expression matches any choice code
            const hasEquals = equalityComparisonRegex.test(expressionToCheck);
            const hasNotEquals = notEqualityComparisonRegex.test(
              expressionToCheck
            );
            const choices = keyword.choices.map((choice) => choice.code);

            const choicesPattern = new RegExp(`\\b(${choices.join("|")})\\b`);

            return (
              (hasEquals || hasNotEquals) &&
              choicesPattern.test(expressionToCheck)
            );
          }
        }
        return false; // Keyword not found, considered valid
      });

      return isValid;
    },
    isValidBasePriceFormula(index) {
      // Get the set of valid keywords from the questions
      const validKeywords = this.questions;

      // Get the base price formula from the calculation rules based on the provided index
      const formula = this.CreateCalculatePrice.basePremiumCalculationRules[
        index
      ].basePriceFormula;

      // Regular expression for matching numbers with optional decimal and 'M' at the end
      const numberPattern = /^\d+(\.\d+)?M$/;

      // Regular expression for matching complex mathematical expressions like '((NUM_OF_CLAIM * 0.2M) + (AREA * 0.3M)) / 2M'
      const codeExpressionPattern = /^(\(.*\)|\w+)\s*([+\-*/])\s*\d+(\.\d+)?M(\s*([+\-*/])\s*\d+(\.\d+)?M)*$/;

      // Check if the formula includes any of the valid keywords
      const isValidKeyword = validKeywords.some(
        (keyword) =>
          formula.includes(keyword.questionCode) &&
          keyword.questionType === "Numeric"
      );

      const insValidNumber = numberPattern.test(formula);

      // Check if the formula matches the number pattern or code expression pattern
      const isValidFormula = codeExpressionPattern.test(formula);

      // Return true if either the keyword or the formula is valid
      return (isValidKeyword && isValidFormula) || insValidNumber;
    },
    isValidParamValue(index) {
      const paramValue = this.CreateCalculatePrice.discountMarkupRules[index]
        .paramValue;
      return paramValue > 0;
    },
    handleSubmit() {
      const data = {
        ...this.CreateCalculatePrice,
        code: this.code.toUpperCase(),
      };

      this.checkValid().then((isValid) => {
        if (!isValid) {
          setTimeout(() => {
            alert("Please complete the entire form.");
          }, 100);
          return;
        } else {
          if (this.mode === "CREATE") {
            HTTP.post("Pricing/create", data);
          } else {
            HTTP.post("Pricing/update", data);
            this.mode = "";
          }
          this.$nextTick(() => {
            (this.activeTab = 0),
              (this.code = ""),
              (this.covers = []),
              (this.questions = []),
              (this.CreateCalculatePrice = {
                code: "",
                basePremiumCalculationRules: [basePremiumCalculationRules],
                discountMarkupRules: [discountMarkupRules],
              });
          });
        }
      });
    },
    async handleUpdate() {
      await HTTP.get("Pricing/" + this.code.toUpperCase()).then(
        async (response) => {
          if (response.data === null) {
            alert(`"Not found with code ${this.code.toUpperCase()}"`);
            return;
          } else {
            await this.handleCodeSubmit();
            this.CreateCalculatePrice = response.data;
            this.mode = "UPDATE";
          }
        }
      );
    },
    async checkValid() {
      const isValidForm = await new Promise((resolve) => {
        this.$nextTick(() => {
          // Add additional validation checks as needed
          const discountMarkupRules = this.CreateCalculatePrice
            .discountMarkupRules;
          const basePremiumCalculationRules = this.CreateCalculatePrice
            .basePremiumCalculationRules;
          const isCoverCode = basePremiumCalculationRules.every(
            (rule, index) => {
              const isValidCoverCode = this.isValidCoverCode(index);
              return isValidCoverCode;
            }
          );
          const isBasePrice = basePremiumCalculationRules.every(
            (rule, index) => {
              const isValidBasePriceFormula = this.isValidApplyIfFormulaInBasePremium(
                index
              );
              return isValidBasePriceFormula;
            }
          );
          const isBaseFormula = basePremiumCalculationRules.every(
            (rule, index) => {
              const isValidBaseFormula = this.isValidBasePriceFormula(index);
              return isValidBaseFormula;
            }
          );
          const isValidDiscount = discountMarkupRules.every((rule, index) => {
            const isValidFormulaInDiscount = this.isValidApplyIfFormulaInDiscount(
              index
            );
            // Thực hiện kiểm tra tính hợp lệ của paramValue tại đây nếu cần
            return isValidFormulaInDiscount;
          });
          const isParamValue = discountMarkupRules.every((rule, index) => {
            const isValidParamValue = this.isValidParamValue(index);
            // Thực hiện kiểm tra tính hợp lệ của paramValue tại đây nếu cần
            return isValidParamValue;
          }); // Your custom paramValue validation

          resolve(
            isCoverCode &&
              isBasePrice &&
              isBaseFormula &&
              isValidDiscount &&
              isParamValue
          );
        });
      });
      return isValidForm;
    },
    addBasePremium() {
      this.CreateCalculatePrice.basePremiumCalculationRules.push({
        ...basePremiumCalculationRules,
        coverCode: "",
        applyIfFormula: "",
        basePriceFormula: "",
      });
    },
    removeBasePremium(index) {
      if (this.CreateCalculatePrice.basePremiumCalculationRules.length === 0) {
        setTimeout(() => {
          alert(
            "Base Premium Calculation Rules must include at least one object."
          );
        }, 100);
      } else
        this.CreateCalculatePrice.basePremiumCalculationRules.splice(index, 1);
    },
    addDiscount() {
      this.CreateCalculatePrice.discountMarkupRules.push({
        ...discountMarkupRules,
        applyIfFormula: "",
        paramValue: 0,
      });
    },
    removeDiscount(index) {
      if (index === 0) {
        setTimeout(() => {
          alert("Discount Markup Rules must include at least one object.");
        }, 100);
      } else this.CreateCalculatePrice.discountMarkupRules.splice(index, 1);
    },
  },
};
</script>
  
<style scoped>
.search_button {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 10px;
  max-width: 120px;
}

.questionContainer {
  display: flex;
  justify-content: center;
  gap: 100px;
}

.questionCode {
  border: 1px solid cyan;
  background-color: aqua;
  font-size: 20px;
}
</style>
