import {Component, Inject} from '@angular/core';
import {CurrencyService} from "../services";
import {CurrencyRate} from "../models/currency.rate";
import { differenceInCalendarDays } from 'date-fns';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public currencyNames: string[] = [];
  public selectedCurrency: string = '';
  public selectedDate: Date = new Date();
  public currentRate: CurrencyRate|null = null;
  today = new Date();

  constructor(private dataService: CurrencyService, @Inject('BASE_URL') baseUrl: string) {
    dataService.getCurrencyNames().subscribe(result => {
      this.currencyNames = result;
    }, error => console.error(error));
  }

  disabledFutureDates = (current: Date): boolean =>
    differenceInCalendarDays(current, this.today) > 0;

  onDateChange(date: Date): void {
    this.selectedDate = date;
    this.getCurrencyRate();
  }

  onCurrencyChange(currency: string): void {
    this.selectedCurrency = currency;
    this.getCurrencyRate();
  }

  private getCurrencyRate(): void {
    if (this.selectedCurrency == '') {
      return
    }

    this.dataService.getCurrencyRate(this.selectedDate, this.selectedCurrency).subscribe(result => {
      this.currentRate = result;
    }, error => console.error(error));

  }
}
