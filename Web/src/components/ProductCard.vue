<template>
  <div>
    <b-card
      :title="`${product.name} (${product.code})`"
      :img-src="imageSrc"
      :img-alt="product.name"
      img-top
      tag="article"
    >
      <p class="card-text">
        {{ product.description }}
        <CoverList :covers="product.covers" />
      </p>
      <b-row>
        <b-col>
          <b-button
            :key="product.code"
            pill 
            variant="info"
            @click="openModal(product.code)"
            v-if="userTypes.includes('ADMIN')"
          >
            Edit
          </b-button>
        </b-col>
        <b-col>
          <router-link :to="{name: 'product', params: { productCode: product.code }}">
            <b-button
              pill
              type="submit"
              variant="primary"
            >
              Buy
            </b-button>
          </router-link>
        </b-col>
      </b-row>
    </b-card>
    <b-modal
      :id="'updateProduct-' + selectedProduct"
      :visible="visible"
      title="Edit Product"
      size="lg"
      @hidden="visible = false"
      @ok="handleOk"
    >
      <b-form
        ref="updateForm"
        :modal="product"
        @submit.stop.prevent
      >
        <ProductUpdate 
          :product="updateProduct" 
          :productImage="productImage"
        />
      </b-form>
    </b-modal>
  </div>
</template>
  
<script>
import CoverList from "./CoverList";
import ProductUpdate from "./ProductUpdate.vue";
import { HTTP } from "./http/ApiClient";
import { product } from "./ProductCreate.vue";
import { DETAILS_USER } from "./http/Auth";

export default {
  name: "ProductCard",
  components: { CoverList, ProductUpdate },
  props: {
    product: {},
    image: "",
  },
  data() {
    return {
      imageSrc: "",
      visible: false,
      selectedProduct: "",
      updateProduct: product,
      productImage: null,
      userTypes: [],
    };
  },
  async created() {
    const response = await HTTP.get("" + this.image, {
      responseType: "arraybuffer",
    });

    // Convert the binary data to a data URI
    const arrayBufferView = new Uint8Array(response.data);
    const blob = new Blob([arrayBufferView], { type: "image/jpeg" });
    const imageUrl = URL.createObjectURL(blob);

    this.imageSrc = imageUrl;

    const user = localStorage.getItem(DETAILS_USER);
    this.userTypes = JSON.parse(user).userTypes;
  },
  methods: {
    async uploadImage() {
      if (this.productImage !== null) {
        const formData = new FormData();
        formData.append("imageFile", this.productImage);
        await HTTP.post("Products/uploads", formData).then((response) => {
          this.productImage = response.data.imageFileName;
        });
        const data = {
          ...this.productDraft,
          image: this.productImage,
        };
        return data;
      } else {
        return this.updateProduct;
      }
    },
    async openModal(productCode) {
      (this.selectedProduct = productCode), (this.visible = !this.visible);
      await this.getProduct(0);
    },
    async getProduct(productId) {
      if (productId === 0) {
        productId = this.selectedProduct;
      }
      await HTTP.get("products/" + productId).then((reponse) => {
        this.updateProduct = reponse.data;
      });
    },
    async handleSubmit() {
      // Kiểm tra tính hợp lệ của form
      const isValid = await this.$refs.updateForm.checkValidity();

      if (isValid) {
        await this.Update();
        this.activeTab = 0;
        this.visible = false;
        window.location.reload();
      } else {
        // Form không hợp lệ, thông báo hoặc xử lý một cách phù hợp
        setTimeout(() => {
          alert("Please complete the entire form.");
        }, 100);
      }
    },
    async Update() {
      const data = await this.uploadImage();
      await HTTP.post("products/update", {
        productDraft: data,
      });
    },
    handleOk(bvModalEvent) {
      bvModalEvent.preventDefault();
      this.handleSubmit();
    },
  },
};
</script>
