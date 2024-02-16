<template>
  <div id="app">
    <div v-if="!auth.isAuthenticated()">
      <div v-if="!registerActive">
        <div class="form-login">
          <b-form
            ref="ruleForm"
            :model="ruleForm"
            style="width: 450px;"
            @submit="handleSubmit()"
          >
            <b-card footer-tag="footer">
              <h2 style="margin-bottom: 30px;">
                <b>Sign In</b>
              </h2>
              <b-form-group
                id="usernamegroup"
                label-for="username"
              >
                <b-form-input
                  id="username"
                  v-model="ruleForm.username"
                  required
                  class="flex-grow-1"
                  placeholder="Username"
                />
              </b-form-group>
              <b-form-group
                id="passwordgroup"
                label-for="password"
              >
                <div class="input-group">
                  <b-form-input
                    id="password"
                    v-model="ruleForm.password"
                    :type="showPassword ? 'text' : 'password'"
                    required
                    placeholder="Password"
                  />
                  <div class="input-group-append">
                    <b-button @click="togglePasswordVisibility" variant="outline-secondary">
                      <b-icon v-if="!showPassword" icon="eye" />
                      <b-icon v-if="showPassword" icon="eye-slash" />
                    </b-button>
                  </div>
                </div>
              </b-form-group>
              <template #footer>
                <b-button type="button" v-on:click="signIn()" variant="outline-primary">Login</b-button>
                <p>Don't have an account? 
                  <a href="#" @click="registerActive = !registerActive">Sign up here</a>
                </p>
                <p><a href="#">Forgot your password?</a></p>
              </template>
            </b-card>
          </b-form>
        </div>
      </div>
      <div v-else>
        <div class="form-login">
          <b-form
            ref="register"
            :model="registerForm"
            style="width: 450px;"
            @submit="handleSubmit()"
          >
            <b-card footer-tag="footer">
              <h2 style="margin-bottom: 30px;">
                <b>Sign Up</b>
              </h2>
              <b-form-group
                id="usernamegroup"
                label-for="username"
              >
                <b-form-input
                  id="username"
                  v-model="registerForm.username"
                  required
                  placeholder="Username"
                />
              </b-form-group>
              <b-form-group
                id="passwordgroup"
                label-for="password"
              >
                <div class="input-group">
                  <b-form-input
                    id="password"
                    v-model="registerForm.password"
                    :type="showRegisterPassword ? 'text' : 'password'"
                    required
                    placeholder="Password"
                  />
                  <div class="input-group-append">
                    <b-button @click="toggleRegisterPasswordVisibility" variant="outline-secondary">
                      <b-icon v-if="!showRegisterPassword" icon="eye" />
                      <b-icon v-if="showRegisterPassword" icon="eye-slash" />
                    </b-button>
                  </div>
                </div>
              </b-form-group>
              <b-form-group
                id="confirm_passwordgroup"
                label-for="confirm_password"
              >
                <div class="input-group">
                  <b-form-input
                    id="confirm_password"
                    v-model="registerForm.confirm_password"
                    :type="showConfirmPassword ? 'text' : 'password'"
                    required
                    placeholder="Confirm Password"
                  />
                  <div class="input-group-append">
                    <b-button @click="toggleConfirmPasswordVisibility" variant="outline-secondary">
                      <b-icon v-if="!showConfirmPassword" icon="eye" />
                      <b-icon v-if="showConfirmPassword" icon="eye-slash" />
                    </b-button>
                  </div>
                </div>
                <b-form-invalid-feedback :state="checkMatch()">
                  The confirm password you entered does not match the password.
                </b-form-invalid-feedback>
              </b-form-group>
              <b-form-group
                id="imagegroup"
                label-for="avt"
              >
                <b-form-file
                  id="avt"
                  v-model="registerForm.avatar"
                  placeholder="Choose a avatar or drop it here..."
                  required
                />
              </b-form-group>
              <template #footer>
                <b-button v-on:click="signUp()" type="button" variant="outline-primary">Register</b-button>
                <p>Already have an account? 
                  <a href="#" @click="registerActive = !registerActive">Sign in here</a>
                </p>
              </template>
            </b-card>
          </b-form>
        </div>
      </div>
    </div>
    <div v-else>
      <b-navbar toggleable="lg" type="dark" variant="info">
        <b-navbar-brand href="/#/">Home</b-navbar-brand>

        <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

        <b-collapse id="nav-collapse" is-nav>
          <b-navbar-nav>
            <b-nav-item v-if="userTypes.includes('ADMIN')" href="/#/dashboard">Dashboard</b-nav-item>
            <b-nav-item href="/#/products">Products</b-nav-item>
            <b-nav-item v-if="userTypes.includes('ADMIN')" href="/#/pricing">Prices</b-nav-item>
            <b-nav-item href="/#/policies">Policies</b-nav-item>
            <b-nav-item v-if="userTypes.includes('ADMIN')" href="/#/payment">Payment</b-nav-item>
            <b-nav-item href="/#/chat">Chat</b-nav-item>
          <!-- <b-nav-item href="/#/account">Account</b-nav-item> -->
          </b-navbar-nav>

          <b-navbar-nav class="ml-auto">
            <template #button-content>
              <div class="header-avt">
                <b-avatar :src="avatarUser || 'https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png'" size="2rem" />
                <span style="margin-left: 8px;">{{ user.login || "abc" }}</span>
              </div>
            </template>
            <b-nav-item-dropdown right>
              <b-dropdown-item href="#">Profile</b-dropdown-item>
              <b-dropdown-item href="#" @click="logout()">Sign Out</b-dropdown-item>
            </b-nav-item-dropdown>
          </b-navbar-nav>
        </b-collapse>
      </b-navbar>
      <hr/>

      <div id="main-container">
        <router-view/>

        <div class="btn-chat" id="livechat-compact-container" style="visibility: visible; opacity: 1;">
          <div class="btn-holder">
            <router-link to="/chatbot" class="link">Live Chat</router-link>
          </div>
        </div>
      </div>

      <footer>
        <img src="./assets/logo_horizontal.png" alt="Logo Footer"/>
        <p><a href="https://asc.altkom.pl">Altkom Software & Consulting LAB @ 2020</a></p>
      </footer>
    </div>
  </div>
</template>

<script>
import auth from "@/components/http/Auth";
import { HTTP } from "@/components/http/ApiClient";

export default {
  name: "Account",
  data() {
    return {
      ruleForm: {
        username: "",
        password: "",
      },
      registerForm: {
        username: "",
        password: "",
        confirm_password: "",
        avatar: null,
      },
      user: {
        avatar: "",
        login: "",
      },
      defaultActive: "/",
      auth: auth,
      registerActive: false,
      userTypes: [],
      showPassword: false,
      showRegisterPassword: false,
      showConfirmPassword: false,
      avatarUser: null,
    };
  },
  async created() {
    if (auth.isAuthenticated()) {
      await this.getUser();

      const response = await auth.avatar(this, this.user.avatar);
      const arrayBufferView = new Uint8Array(response.data);
      const blob = new Blob([arrayBufferView], { type: "image/jpeg" });
      this.avatarUser = URL.createObjectURL(blob);
    }
  },
  methods: {
    handleSubmit(event) {
      event.preventDefault();
      if (!this.registerActive) this.signIn();
      else this.signUp();
    },
    async signUp() {
      this.checkValid().then(async (isvalid) => {
        if (!isvalid) {
          setTimeout(() => {
            alert("Please complete the entire form.");
          }, 100);
          return;
        } else {
          const data = await this.uploadAvatar();
          const credentials = {
            login: data.username,
            password: data.password,
            avatar: data.avatar,
          };
          await auth.register(this, credentials);
          window.location.reload();
        }
      });
    },
    async uploadAvatar() {
      const formData = new FormData();
      formData.append("file", this.registerForm.avatar);
      const avt = await auth.postAvt(this, formData);
      const data = {
        ...this.registerForm,
        avatar: avt,
      };
      return data;
    },
    checkValid() {
      return new Promise((resolve) => {
        this.$nextTick(() => {
          var isValid = false;
          if (!this.registerActive)
            isValid = this.$refs.ruleForm.checkValidity();
          else isValid = this.$refs.register.checkValidity();
          resolve(isValid);
        });
      });
    },
    signIn() {
      this.checkValid().then((isvalid) => {
        if (!isvalid) {
          setTimeout(() => {
            alert("Please complete the entire form.");
          }, 100);
          return;
        } else {
          const credentials = {
            login: this.ruleForm.username,
            password: this.ruleForm.password,
          };
          auth
            .login(this, credentials)
            .then(async () => {
              await this.getUser();
            })
            .then(() => {
              auth.getAuthDetails(this.user);
            })
            .then(() => {
              window.location.href = "/";
            });
        }
      });
    },
    logout() {
      auth.logout();
      window.location.reload();
    },
    handleMenuSelect(index) {
      this.$router.push(index);
    },
    async getUser() {
      if (!auth.isAuthenticated()) return;
      const API_URL = process.env.VUE_APP_AUTH_URL;
      const LOGIN_URL = API_URL + "User";
      await HTTP.get(LOGIN_URL).then((response) => {
        this.user = response.data;
        this.userTypes = this.user.userTypes;
      });
    },
    checkMatch() {
      return this.registerForm.password === this.registerForm.confirm_password;
    },
    togglePasswordVisibility() {
      this.showPassword = !this.showPassword;
    },
    toggleRegisterPasswordVisibility() {
      this.showRegisterPassword = !this.showRegisterPassword;
    },
    toggleConfirmPasswordVisibility() {
      this.showConfirmPassword = !this.showConfirmPassword;
    },
  },
};
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

#nav {
  padding: 10px 10px 0 10px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}

#main-container {
  width: 80%;
  margin: 0 auto;
}

footer {
  text-align: center;
  border-top: 1px gray solid;
  margin-left: 10em;
  margin-right: 10em;
  margin-top: 30px;
}

footer > img {
  margin-top: 20px;
  height: 60px;
}

footer > hr {
  width: 90%;
}

footer > p > a {
  color: black;
}

#livechat-compact-container {
  height: 153px;
  position: fixed;
  right: 0;
  top: 30vh;
  z-index: 10000;
}
.btn-chat a {
  color: #fff;
  text-decoration: none;
  background: #1798f7;
  padding: 24px 20px 8px;
  display: block;
  font-weight: bold;
  font-size: 14px;
  box-shadow: 0 0 0 1px #03b2ff inset;
  border: 1px solid #144866;
  border-radius: 2px;
  -ms-transform: rotate(90deg) translate(0, -20px);
  -webkit-transform: rotate(90deg) translate(0, -20px);
  transform: rotate(90deg) translate(0, -20px);
  position: relative;
  right: -27px;
  transition: background 0.2s, right 0.2s;
}
.btn-chat a:hover {
  background: #47b6f5;
  right: -20px;
  transition: background 0.2s, right 0.2s;
}
.header-avt {
  display: flex;
  justify-content: end;
  align-items: center;
}

.form-login {
  height: 100vh;
  width: 100vw;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: cyan;
}

.input-group {
  position: relative;
}

.input-group-append {
  position: relative;
  right: 0;
  top: 0;
  bottom: 0;
  display: flex;
  align-items: center;
}
</style>
