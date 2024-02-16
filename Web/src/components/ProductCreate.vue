<template>
  <b-modal
    id="createProduct"
    ref="createProduct"
    title="Create Product"
    size="lg"
    @cancel="reset"
    @ok="handleOk"
  >
    <b-form
      ref="form"
      :model="productDraft"
      @submit.stop.prevent="handleSubmit"
    >
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
                  label="Code"
                  label-for="code"
                >
                  <b-form-input
                    id="code"
                    v-model="productDraft.code"
                    :state="Boolean(productDraft.code)"
                    required
                  />
                  <b-form-invalid-feedback :state="Boolean(productDraft.code)">
                    Please ensure that your code is not left empty.
                  </b-form-invalid-feedback>
                </b-form-group>
              </b-col>
              <b-col md="6">
                <b-form-group
                  label="Name"
                  label-for="name"
                >
                  <b-form-input
                    id="name"
                    v-model="productDraft.name"
                    :state="Boolean(productDraft.name)"
                    required
                  />
                  <b-form-invalid-feedback :state="Boolean(productDraft.name)">
                    Please ensure that your name is not left empty.
                  </b-form-invalid-feedback>
                </b-form-group>
              </b-col>
            </b-row>
            <b-row class="mt-3">
              <b-col>
                <b-form-group
                  label="Description"
                  label-for="description"
                >
                  <b-form-input
                    id="description"
                    v-model="productDraft.description"
                    :state="Boolean(productDraft.description)"
                    required
                  />
                  <b-form-invalid-feedback :state="Boolean(productDraft.description)">
                    Please ensure that your description is not left empty.
                  </b-form-invalid-feedback>
                </b-form-group>
              </b-col>
            </b-row>
            <b-row class="mt-3">
              <b-col md="6">
                <b-form-group
                  label="Max Number Of Insured"
                  label-for="maxNumberOfInsured"
                >
                  <b-form-input
                    id="maxNumberOfInsured"
                    v-model="productDraft.maxNumberOfInsured"
                    :state="Boolean(productDraft.maxNumberOfInsured > 0)"
                    type="number"
                    required
                  />
                  <b-form-invalid-feedback :state="Boolean(productDraft.maxNumberOfInsured > 0)">
                    Please make sure that the maximum number of insured individuals is set to a value greater than zero.
                  </b-form-invalid-feedback>
                </b-form-group>
              </b-col>
              <b-col md="6">
                <b-form-group
                  label="Image"
                  label-for="image"
                >
                  <b-form-file
                    v-model="productDraft.image"
                    :state="Boolean(productDraft.image)"
                    placeholder="Choose a image or drop it here..."
                    required
                  />
                  <b-form-invalid-feedback :state="Boolean(productDraft.image)">
                    Please make sure that the image is not left empty.
                  </b-form-invalid-feedback>
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
              v-for="(cover, index) in productDraft.covers"
              :key="index"
              class="mb-5"
            >
              <b-col md="2">
                <b-form-group
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
                  label="Name"
                  :label-for="`coverName${index}`"
                >
                  <b-form-input
                    :id="`coverName${index}`"
                    v-model="cover.name"
                    :state="Boolean(cover.name)"
                    required
                  />
                  <b-form-invalid-feedback :state="Boolean(cover.name)">
                    Please ensure that your cover name is not left empty.
                  </b-form-invalid-feedback>
                </b-form-group>
              </b-col>
              <b-col md="2">
                <b-form-group
                  label="Optional"
                  :label-for="`optional${index}`"
                >
                  <b-form-checkbox
                    :id="`optional${index}`"
                    v-model="cover.optional"
                  />
                </b-form-group>
              </b-col>
              <b-col md="4">
                <b-form-group
                  label="Sum Insured"
                  :label-for="`sumInsured${index}`"
                >
                  <b-form-input
                    :id="`sumInsured${index}`"
                    v-model="cover.sumInsured"
                    type="number"
                  />
                </b-form-group>
              </b-col>
              <b-col md="10">
                <b-form-group
                  label="Description"
                  :label-for="`coverDescription${index}`"
                >
                  <b-form-input
                    :id="`coverDescription${index}`"
                    v-model="cover.description"
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
              v-for="(question, index) in productDraft.questions"
              :key="index"
              class="mb-5"
            >
              <b-col md="4">
                <b-form-group
                  label="Question Code"
                  label-for="questionCode"
                >
                  <b-form-input
                    id="questionCode"
                    v-model="question.questionCode"
                    :state="Boolean(question.questionCode)"
                    required
                  />
                  <b-form-invalid-feedback :state="Boolean(question.questionCode)">
                    Please ensure that your question code is not left empty.
                  </b-form-invalid-feedback>
                </b-form-group>
              </b-col>
              <b-col md="2">
                <b-form-group
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
                  label="Text"
                  label-for="questionText"
                >
                  <b-form-input
                    id="questionText"
                    v-model="question.text"
                    :state="Boolean(question.text)"
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
                v-if="question.questionType === 0"
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
    </b-form>
  </b-modal>
</template>
  
<script>
import { HTTP } from "./http/ApiClient";

export const QuestionType = {
  selectType: -1,
  Choice: 0,
  Date: 1,
  Numeric: 2,
  YesNoChoice: 3,
};

export const ListChoice = {
  code: "",
  label: "",
};

export const product = {
  code: "",
  name: "",
  description: "",
  image: null,
  covers: [
    {
      code: "",
      name: "",
      description: "",
      optional: false,
      sumInsured: 0,
    },
  ],
  questions: [
    {
      index: 0,
      questionCode: "",
      text: "",
      questionType: QuestionType.selectType,
      choices: [ListChoice],
    },
  ],
  maxNumberOfInsured: 0,
  icon: "",
};

export default {
  name: "CreateProduct",
  data() {
    return {
      activeTab: 0,
      productDraft: product,
      imageProduct: "",
      productId: "",
      questionTypeOptions: [
        { value: QuestionType.selectType, text: "Select Type" },
        { value: QuestionType.Choice, text: "Choice" },
        { value: QuestionType.YesNoChoice, text: "Yes/No Choice" },
        { value: QuestionType.Date, text: "Date" },
        { value: QuestionType.Numeric, text: "Numeric" },
      ],
    };
  },
  mounted() {
    this.initializeCoverCode();
    this.initialQuestionIndex();
  },
  methods: {
    addCover() {
      this.productDraft.covers.push({
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
      } else this.productDraft.covers.splice(index, 1);
    },
    addQuestion() {
      this.productDraft.questions.push({
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
      } else this.productDraft.questions.splice(index, 1);
    },
    addChoice(index) {
      this.productDraft.questions[index].choices.push({ code: "", label: "" });
    },
    removeChoice(index, choiceIndex) {
      if (choiceIndex === 0)
        setTimeout(() => {
          alert("Choices must include at least one object.");
        }, 100);
      else this.productDraft.questions[index].choices.splice(choiceIndex, 1);
    },
    generateCode(index) {
      return `C${index + 1}`;
    },
    checkValid() {
      return new Promise((resolve) => {
        this.$nextTick(() => {
          const isValid = this.$refs.form.checkValidity();
          resolve(isValid);
        });
      });
    },
    handleSubmit() {
      this.checkValid().then(async (isValid) => {
        if (!isValid) {
          setTimeout(() => {
            alert("Please complete the entire form.");
          }, 100);
          return;
        } else {
          await this.postProduct();
          await this.activate();
          this.activeTab = 0;
          this.$nextTick(() => {
            this.reset();
            this.$refs.createProduct.hide();
          });
          window.location.reload();
        }
      });
    },
    async uploadImage() {
      const formData = new FormData();
      formData.append("imageFile", this.productDraft.image);
      await HTTP.post("Products/uploads", formData).then((response) => {
        this.productImage = response.data.imageFileName;
      });
      const data = {
        ...this.productDraft,
        image: this.productImage,
      };
      data.questions.forEach((question) => {
        if (question.questionType === 1 || question.questionType === 2)
          delete question.choices;
      });
      data.questions = data.questions.map((question) => {
        if (question.questionType === 1 || question.questionType === 2) {
          return {
            ...question,
            questionType: question.questionType === 1 ? "Date" : "Numeric",
          };
        }
        if (question.questionType === 3) {
          return {
            ...question,
            questionType: "Choice",
            choices: [
              { code: "YES", label: "Yes" },
              { code: "NO", label: "No" },
            ],
          };
        }
        return question;
      });
      return data;
    },
    async postProduct() {
      const data = await this.uploadImage();
      await HTTP.post("Products", {
        productDraft: data,
      }).then((response) => {
        this.productId = response.data.productId;
      });
    },
    async activate() {
      await HTTP.post("products/activate", { productId: this.productId });
    },
    handleOk(bvModalEvent) {
      bvModalEvent.preventDefault();
      this.handleSubmit();
    },
    reset() {
      (this.imageProduct = ""),
        (this.productDraft = {
          code: "",
          name: "",
          description: "",
          image: [],
          covers: [
            {
              code: "",
              name: "",
              description: "",
              optional: false,
              sumInsured: 0,
            },
          ],
          questions: [
            {
              index: 0,
              questionCode: "",
              text: "",
              questionType: QuestionType.selectType,
              choices: [ListChoice],
            },
          ],
          maxNumberOfInsured: 0,
          icon: "",
        });
    },
    initializeCoverCode() {
      this.productDraft.covers.forEach((cover, index) => {
        cover.code = this.generateCode(index);
      });
    },
    initialQuestionIndex() {
      this.productDraft.questions.forEach((question, index) => {
        question.index = index + 1;
      });
    },
  },
};
</script>
  <style>
</style>
