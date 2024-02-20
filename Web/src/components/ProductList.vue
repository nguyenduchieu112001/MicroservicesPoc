<template>
  <div>
    <b-row class="mb-4">
      <b-col cols="10">
        <h1>List Products</h1>
      </b-col>
      <b-col
        v-if="userTypes.includes('ADMIN')"
        cols="2"
        class="d-flex align-items-center justify-content-end"
      >
        <b-button
          variant="success"
          class="rounded-circle"
          v-b-modal.createProduct
        >
          <i class="bi bi-plus" />
        </b-button>
      </b-col>
    </b-row>
    <div class="row">
      <div
        v-for="product in products"
        :key="product.code"
        class="col-md-4 mb-5"
      >
        <ProductCard
          :product="product"
          :image="product.image"
          :userTypes="userTypes"
        />
      </div>
    </div>
    <CreateProduct />
  </div>
</template>
  
  <script>
import { HTTP } from "./http/ApiClient";
import CreateProduct from "./ProductCreate.vue";
import ProductCard from "./ProductCard";
import { DETAILS_USER } from "./http/Auth";

export default {
  name: "ProductList",
  components: { ProductCard, CreateProduct },
  data() {
    return {
      activeTab: 0,
      products: [],
      productDrafts: [],
      userTypes: [],
    };
  },
  async created() {
    await this.getProduct();
    const user = localStorage.getItem(DETAILS_USER);
    this.userTypes = JSON.parse(user).userTypes;
  },
  methods: {
    async getProduct() {
      await HTTP.get("Products").then((response) => {
        this.products = response.data;
      });
    },
  },
};
</script>
  
  <style scoped lang="scss">
</style>
