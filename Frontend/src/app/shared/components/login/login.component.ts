import { HttpErrorResponse, HttpHeaderResponse, HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/core/services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm !: FormGroup;

  constructor(private formBuilder: FormBuilder, private router: Router, private userService: UserService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required, Validators.pattern(/(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&^_-]).{8,}/)]]
    });
  }

  get Email(): FormControl {
    return this.loginForm.get('email') as FormControl;
  }

  get Password(): FormControl {
    return this.loginForm.get('password') as FormControl;
  }

  login = () => {
    if (this.loginForm.valid) {
      this.userService.login(this.loginForm.value)
        .subscribe({
          next: (response) => {
              localStorage.setItem("loggedIn", response.loginSuccessful);
              localStorage.setItem("userInfo", JSON.stringify(response.userInfo));
              if (response.userInfo.isAdmin == true) {
                this.router.navigate(['/dashboard']);
              } else {
                this.router.navigate(['/home']);
              }
              this.toastr.success("Login Successful", "Success!");
            
          },
          error: (err: HttpErrorResponse) => {
            console.log(err);
            this.toastr.error("Login Unsuccessful. Please try again.", "Error!");
          }
        })
    }
  }
}
