<template>
  <!-- <b-form
    ref="updateForm"
    :modal="product"
    @submit.stop.prevent
  > -->
  <b-tabs
    v-model="activeTab"
    content-class="mt-3"
    fill
  >
    <b-tab title="Info">
      <b-card>
        <b-row>
          <b-col md="6">
            <b-form-group
              id="info_code"
              label="Code"
              label-for="code"
            >
              <b-form-input
                id="code"
                v-model="product.code"
                :value="product.code"
                :state="Boolean(product.code)"
                required
              />
              <b-form-invalid-feedback :state="Boolean(product.code)">
                Please ensure that your code is not left empty.
              </b-form-invalid-feedback>
            </b-form-group>
          </b-col>
          <b-col md="6">
            <b-form-group
              id="info_name"
              label="Name"
              label-for="name"
            >
              <b-form-input
                id="name"
                v-model="product.name"
                :state="Boolean(product.name)"
                :value="product.name"
                required
              />
              <b-form-invalid-feedback :state="Boolean(product.name)">
                Please ensure that your name is not left empty.
              </b-form-invalid-feedback>
            </b-form-group>
          </b-col>
        </b-row>
        <b-row class="mt-3">
          <b-col>
            <b-form-group
              id="info_description"
              label="Description"
              label-for="description"
            >
              <b-form-input
                id="description"
                v-model="product.description"
                :state="Boolean(product.description)"
                :value="product.description"
                required
              />
              <b-form-invalid-feedback :state="Boolean(product.description)">
                Please ensure that your description is not left empty.
              </b-form-invalid-feedback>
            </b-form-group>
          </b-col>
        </b-row>
        <b-row class="mt-3">
          <b-col md="6">
            <b-form-group
              id="info_maxNumber"
              label="Max Number Of Insured"
              label-for="maxNumberOfInsured"
            >
              <b-form-input
                id="maxNumberOfInsured"
                v-model="product.maxNumberOfInsured"
                :state="Boolean(product.maxNumberOfInsured > 0)"
                :value="product.maxNumberOfInsured"
                type="number"
                required
              />
              <b-form-invalid-feedback :state="Boolean(product.maxNumberOfInsured > 0)">
                Please make sure that the maximum number of insured individuals is set to a value greater than zero.
              </b-form-invalid-feedback>
            </b-form-group>
          </b-col>
          <b-col md="6">
            <b-form-group
              id="info_image"
              label="Image"
              label-for="image"
            >
              <b-form-file
                v-model="productImage"
                placeholder="Choose a image or drop it here..."
              />
            </b-form-group>
          </b-col>
        </b-row>
      </b-card>
    </b-tab>
    <b-tab title="Covers">
      <b-card>
        <b-row class="mb-3">
          <b-col>
            <b-button
              class="float-right rounded-circle"
              variant="success"
              @click="addCover"
            >
              <i class="bi bi-plus" />
            </b-button>
          </b-col>
        </b-row>
        <b-row
          v-for="(cover, index) in product.covers"
          :key="index"
          class="mb-5"
        >
          <b-col md="2">
            <b-form-group
              id="cover_code"
              label="Code"
              :label-for="`coverCode${index}`"
            >
              <b-form-input
                :id="`coverCode${index}`"
                v-model="cover.code" 
                :value="generateCode(index)"
                :disabled="true"
                required
              />
            </b-form-group>
          </b-col>
          <b-col md="4">
            <b-form-group
              id="cover_name"
              label="Name"
              :label-for="`coverName${index}`"
            >
              <b-form-input
                :id="`coverName${index}`"
                v-model="cover.name"
                :state="Boolean(cover.name)"
                :value="cover.name"
                required
              />
              <b-form-invalid-feedback :state="Boolean(cover.name)">
                Please ensure that your cover name is not left empty.
              </b-form-invalid-feedback>
            </b-form-group>
          </b-col>
          <b-col md="2">
            <b-form-group
              id="cover_optional"
              label="Optional"
              :label-for="`optional${index}`"
            >
              <b-form-checkbox
                :id="`optional${index}`"
                v-model="cover.optional"
                :checked="cover.optional"
              />
            </b-form-group>
          </b-col>
          <b-col md="4">
            <b-form-group
              id="cover_sumInsured"
              label="Sum Insured"
              :label-for="`sumInsured${index}`"
            >
              <b-form-input
                :id="`sumInsured${index}`"
                v-model="cover.sumInsured"
                type="number"
                :value="cover.sumInsured"
              />
            </b-form-group>
          </b-col>
          <b-col md="10">
            <b-form-group
              id="cover_description"
              label="Description"
              :label-for="`coverDescription${index}`"
            >
              <b-form-input
                :id="`coverDescription${index}`"
                v-model="cover.description"
                :value="cover.description"
              />
            </b-form-group>
          </b-col>
          <b-col
            md="2"
            style="display: flex; justify-content: center; align-items: center; margin-top: 16px;"
          >
            <b-button
              class="float-right"
              variant="danger"
              @click="removeCover(index)"
            >
              <i class="bi bi-dash-lg" />
            </b-button>
          </b-col>
        </b-row> 
      </b-card>
    </b-tab>
    <b-tab title="Questions">
      <b-card>
        <b-row class="mb-3">
          <b-col>
            <b-button
              class="float-right rounded-circle"
              variant="success"
              @click="addQuestion"
            >
              <i class="bi bi-plus" />
            </b-button>
          </b-col>
        </b-row>
        <b-row
          v-for="(question, index) in product.questions"
          :key="index"
          class="mb-5"
        >
          <b-col md="4">
            <b-form-group
              id="question_code"
              label="Question Code"
              label-for="questionCode"
            >
              <b-form-input
                id="questionCode"
                v-model="question.questionCode"
                :state="Boolean(question.questionCode)"
                :value="question.questionCode"
                required
              />
              <b-form-invalid-feedback :state="Boolean(question.questionCode)">
                Please ensure that your question code is not left empty.
              </b-form-invalid-feedback>
            </b-form-group>
          </b-col>
          <b-col md="2">
            <b-form-group
              id="question_index"
              label="Index"
              label-for="questionIndex"
            >
              <b-form-input
                id="questionIndex"
                v-model="question.index"
                :value="index + 1"
                disabled
              />
            </b-form-group>
          </b-col>
          <b-col md="5">
            <b-form-group
              id="question_text"
              label="Text"
              label-for="questionText"
            >
              <b-form-input
                id="questionText"
                v-model="question.text"
                :state="Boolean(question.text)"
                :value="question.text"
                required
              />
              <b-form-invalid-feedback :state="Boolean(question.text)">
                Please ensure that your question text is not left empty.
              </b-form-invalid-feedback>
            </b-form-group>
          </b-col>
          <b-col
            md="1"
            style="display: flex; justify-content: center; align-items: center;"
          >
            <b-button
              class="float-right"
              variant="danger"
              @click="removeQuestion(index)"
            >
              <i class="bi bi-dash-lg" />
            </b-button>
          </b-col>
          <b-col md="4">
            <b-form-group
              id="question_type"
              label="Question Type"
              label-for="questionType"
            >
              <b-form-select
                id="questionType"
                v-model="question.questionType"
                :options="questionTypeOptions"
                :state="Boolean(question.questionType !== -1)"

              />
              <b-form-invalid-feedback :state="Boolean(question.questionType !== -1)">
                Please select question type.
              </b-form-invalid-feedback>
            </b-form-group>
          </b-col>
          <b-col
            v-if="question.questionType === 'Choice'"
            md="8"
          >
            <b-card>
              <b-row class="mt-2">
                <b-col>
                  <b-button
                    class="float-right rounded-circle"
                    variant="success"
                    @click="addChoice(index)"
                  >
                    <i class="bi bi-plus" />
                  </b-button>
                </b-col>
              </b-row>
              <b-row
                v-for="(choice, choiceIndex) in question.choices"
                :key="choiceIndex"
                class="mb-3"
              >
                <b-col md="4">
                  <b-form-group
                    label="Code"
                    :label-for="`choiceCode${choiceIndex}`"
                  >
                    <b-form-input
                      :id="`choiceCode${choiceIndex}`"
                      v-model="choice.code"
                      :state="Boolean(choice.code)"
                    />
                    <b-form-invalid-feedback :state="Boolean(choice.code)">
                      Please ensure that your choice code is not left empty.
                    </b-form-invalid-feedback>
                  </b-form-group>
                </b-col>
                <b-col md="4">
                  <b-form-group
                    label="Label"
                    :label-for="`choiceLabel${choiceIndex}`"
                  >
                    <b-form-input
                      :id="`choiceLabel${choiceIndex}`"
                      v-model="choice.label"
                      :state="Boolean(choice.label)"
                    />
                    <b-form-invalid-feedback :state="Boolean(choice.label)">
                      Please ensure that your choice label is not left empty.
                    </b-form-invalid-feedback>
                  </b-form-group>
                </b-col>
                <b-col
                  md="4"
                  style="display: flex; justify-content: center; align-items: center;"
                >
                  <b-button
                    class="float-right"
                    variant="danger"
                    @click="removeChoice(index, choiceIndex)"
                  >
                    <i class="bi bi-dash-lg" />
                  </b-button>
                </b-col>
              </b-row>
            </b-card>
          </b-col>
        </b-row>
      </b-card>
    </b-tab>
  </b-tabs>
  <!-- </b-form> -->
</template>

<script>
import { QuestionType } from "./ProductCreate.vue";

export default {
  name: "UpdateProduct",
  props: {
    product: {},
    productImage: null,
  },
  data() {
    return {
      activeTab: 0,
      questionTypeOptions: [
        { value: "selectType", text: "Select Type" },
        { value: "Choice", text: "Choice" },
        { value: "Date", text: "Date" },
        { value: "Numeric", text: "Numeric" },
      ],
    };
  },
  methods: {
    generateCode(index) {
      return `C${index + 1}`;
    },
    addCover() {
      this.product.covers.push({
        code: "",
        name: "",
        description: "",
        optional: false,
        sumInsured: 0,
      });
      this.initializeCoverCode();
    },
    removeCover(index) {
      if (index === 0) {
        setTimeout(() => {
          alert("Covers must include at least one object.");
        }, 100);
      } else this.product.covers.splice(index, 1);
    },
    addQuestion() {
      this.product.questions.push({
        questionCode: "",
        index: 0,
        text: "",
        questionType: QuestionType.selectType,
        choices: [
          {
            code: "",
            label: "",
          },
        ],
      });
      this.initialQuestionIndex();
    },
    removeQuestion(index) {
      if (index === 0) {
        setTimeout(() => {
          alert("Questions must include at least one object.");
        }, 100);
      } else this.product.questions.splice(index, 1);
    },
    addChoice(index) {
      this.product.questions[index].choices.push({ code: "", label: "" });
    },
    removeChoice(index, choiceIndex) {
      if (choiceIndex === 0)
        setTimeout(() => {
          alert("Choices must include at least one object.");
        }, 100);
      else this.product.questions[index].choices.splice(choiceIndex, 1);
    },
  },
};
</script>
