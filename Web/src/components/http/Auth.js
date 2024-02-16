const API_URL = process.env.VUE_APP_AUTH_URL
  ? process.env.VUE_APP_AUTH_URL
  : "/login/";
const API_AVATAR_URL = `${process.env.VUE_APP_AUTH_URL}avatar`;
const LOGIN_URL = API_URL + "User";
const REGISTER_URL = API_URL + "User/signup";
const REGISTER_AVATAR_URL = API_URL + "User/avatar";

export const TOKEN_KEY = "jwt";
export const DETAILS_KEY = "auth-details";
export const DETAILS_USER = "user-details";

export default {
  login(context, credentials) {
    this.clearToken();
    return context.$http.post(LOGIN_URL, credentials).then(
      (response) => {
        localStorage.setItem(TOKEN_KEY, response.body.token);
        localStorage.setItem(DETAILS_KEY, JSON.stringify(response.body));
      },
      (error) => {
        if (error.status === 400) {
          alert(error.data.message);
        }
      }
    );
  },

  register(context, credentials) {
    this.clearToken();
    return context.$http.post(REGISTER_URL, credentials).then((error) => {
      if (error.status === 400) {
        alert(error.data.message);
      } else window.location.reload();
    });
  },

  avatar(context, credentials) {
    return context.$http.get(API_AVATAR_URL + credentials, {
      responseType: "arraybuffer",
    });
  },

  postAvt(context, credentials) {
    return context.$http
      .post(REGISTER_AVATAR_URL, credentials)
      .then((response) => {
        return response.body;
      });
  },

  logout() {
    this.clearToken();
  },

  clearToken() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(DETAILS_KEY);
    localStorage.removeItem(DETAILS_USER);
  },

  isAuthenticated() {
    return localStorage.getItem(TOKEN_KEY) != null;
  },

  getAuthDetails(user) {
    if (!this.isAuthenticated()) return null;
    localStorage.setItem(DETAILS_USER, JSON.stringify(user));
  },
};
