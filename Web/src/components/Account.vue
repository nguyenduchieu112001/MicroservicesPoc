<template>
  <div class="mx-auto" style="max-width: 20rem;">
    <div v-if="!auth.isAuthenticated()">
      <b-form>
        <b-card header-tag="header" footer-tag="footer" style="width: 540px;">
          <template #header>
            <h2>Log in to your account</h2>
          </template>
          <b-form-group id="usernameGroup"
                        label="Username:"
                        label-for="userName">
            <b-form-input id="userName"
                          type="text"
                          v-model="credentials.username"
                          required
                          placeholder="Enter username">
            </b-form-input>
          </b-form-group>

          <b-form-group id="passwordGroup"
                        label="Password:"
                        label-for="password">
            <b-form-input id="password"
                          type="password"
                          v-model="credentials.password"
                          required
                          placeholder="Enter password">
            </b-form-input>
          </b-form-group>
          <template #footer>
            <b-button type="button" v-on:click="login()" variant="primary">Login</b-button>
          </template>
        </b-card>
      </b-form>

    </div>
    <div v-else>
      <div class="header-avt">
        <b-avatar :src="user.avatar || 'https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png'" size="4rem" />
        <span style="margin-left: 8px;">{{ user.login || "abc" }}</span>
        <b-form>
          <b-button style="margin-left: 20px;" type="button" v-on:click="logout()" variant="primary">Logout</b-button>
        </b-form>
      </div>
    </div>
  </div>

</template>

<script>
import auth from "./http/Auth";
import { HTTP } from "./http/ApiClient";

export default {
  name: "Account",
  data() {
    return {
      credentials: {
        username: "",
        password: "",
      },
      user: {
        login: "",
        avatar: "",
        role: [],
        userType: "",
      },
      error: "",
      auth: auth,
    };
  },
  async created() {
    if (auth.isAuthenticated()) {
      await this.getUser();
    }
  },
  methods: {
    login() {
      const credentials = {
        login: this.credentials.username,
        password: this.credentials.password,
      };
      auth
        .login(this, credentials)
        .then(() => {
          return this.getUser();
        })
        .then(() => {
          auth.getAuthDetails(this.user);
        })
        .then(() => {
          window.location.href = "/";
        });
    },
    logout() {
      auth.logout();
      window.location.reload();
    },
    async getUser() {
      const API_URL = process.env.VUE_APP_AUTH_URL;
      const LOGIN_URL = API_URL + "User";
      await HTTP.get(LOGIN_URL).then((response) => {
        this.user = response.data;
      });
    },
  },
};
</script>
<style>
.header-avt {
  display: flex;
  justify-content: end;
  align-items: center;
}
</style>
