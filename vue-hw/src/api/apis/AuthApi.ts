// tslint:disable
/**
 * JWTToken_Auth_API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

import type { Observable } from 'rxjs';
import type { AjaxResponse } from 'rxjs/ajax';
import { BaseAPI } from '../runtime';
import type { OperationOpts, HttpHeaders } from '../runtime';
import type {
    LoginVM,
    RegisterVM,
} from '../models';

export interface ApiAuthLoginPostRequest {
    loginVM?: LoginVM;
}

export interface ApiAuthRegisterPostRequest {
    registerVM?: RegisterVM;
}

/**
 * no description
 */
export class AuthApi extends BaseAPI {

    /**
     */
    apiAuthLoginPost({ loginVM }: ApiAuthLoginPostRequest): Observable<void>
    apiAuthLoginPost({ loginVM }: ApiAuthLoginPostRequest, opts?: OperationOpts): Observable<void | AjaxResponse<void>>
    apiAuthLoginPost({ loginVM }: ApiAuthLoginPostRequest, opts?: OperationOpts): Observable<void | AjaxResponse<void>> {

        const headers: HttpHeaders = {
            'Content-Type': 'application/json',
            ...(this.configuration.apiKey && { 'Authorization': this.configuration.apiKey('Authorization') }), // Bearer authentication
        };

        return this.request<void>({
            url: '/api/Auth/Login',
            method: 'POST',
            headers,
            body: loginVM,
        }, opts?.responseOpts);
    };

    /**
     */
    apiAuthLogoutPost(): Observable<void>
    apiAuthLogoutPost(opts?: OperationOpts): Observable<void | AjaxResponse<void>>
    apiAuthLogoutPost(opts?: OperationOpts): Observable<void | AjaxResponse<void>> {
        const headers: HttpHeaders = {
            ...(this.configuration.apiKey && { 'Authorization': this.configuration.apiKey('Authorization') }), // Bearer authentication
        };

        return this.request<void>({
            url: '/api/Auth/Logout',
            method: 'POST',
            headers,
        }, opts?.responseOpts);
    };

    /**
     */
    apiAuthRegisterPost({ registerVM }: ApiAuthRegisterPostRequest): Observable<void>
    apiAuthRegisterPost({ registerVM }: ApiAuthRegisterPostRequest, opts?: OperationOpts): Observable<void | AjaxResponse<void>>
    apiAuthRegisterPost({ registerVM }: ApiAuthRegisterPostRequest, opts?: OperationOpts): Observable<void | AjaxResponse<void>> {

        const headers: HttpHeaders = {
            'Content-Type': 'application/json',
            ...(this.configuration.apiKey && { 'Authorization': this.configuration.apiKey('Authorization') }), // Bearer authentication
        };

        return this.request<void>({
            url: '/api/Auth/Register',
            method: 'POST',
            headers,
            body: registerVM,
        }, opts?.responseOpts);
    };

}
