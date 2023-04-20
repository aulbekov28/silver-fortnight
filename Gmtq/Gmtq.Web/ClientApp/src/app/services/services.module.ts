import {  HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import {CurrencyService} from "./currency.service";

@NgModule({
  imports: [HttpClientModule],
  providers: [
    CurrencyService,
  ]
})
export class ServicesModule { }
