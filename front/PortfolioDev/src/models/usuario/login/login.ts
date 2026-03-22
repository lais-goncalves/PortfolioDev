export class Login {
    userNameOuEmail: string;
    password: string;

    constructor(userNameOuEmail: string, password: string) {
      this.userNameOuEmail = userNameOuEmail;
      this.password = password;
    }
}
