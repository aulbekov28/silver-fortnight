import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CurrencyRate } from "../models/currency.rate";
import { API_BASE_URL } from '../config';

@Injectable()
export class CurrencyService {

  constructor(private http: HttpClient) {
  }

  getCurrencyNames(): Observable<string[]> {
    return this.http.get<string[]>(`${API_BASE_URL}/CurrencyRates/list`);
  }

  getCurrencyRate(date: Date, currency: string): Observable<any> {
    return this.http.get<CurrencyRate>(`${API_BASE_URL}/CurrencyRates/${currency}/${date.toISOString()}`);
  }

}
