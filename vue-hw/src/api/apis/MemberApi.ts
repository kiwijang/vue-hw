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
import type { OperationOpts, HttpHeaders, HttpQuery } from '../runtime';
import type {
    MemberVM,
} from '../models';

export interface ApiMemberGetMemberByUserIdPostRequest {
    id?: string;
}

export interface ApiMemberUpdateMemberPostRequest {
    memberVM?: MemberVM;
}

/**
 * no description
 */
export class MemberApi extends BaseAPI {

    /**
     */
    apiMemberGetAllMembersPost(): Observable<Array<MemberVM>>
    apiMemberGetAllMembersPost(opts?: OperationOpts): Observable<AjaxResponse<Array<MemberVM>>>
    apiMemberGetAllMembersPost(opts?: OperationOpts): Observable<Array<MemberVM> | AjaxResponse<Array<MemberVM>>> {
        const headers: HttpHeaders = {
            ...(this.configuration.apiKey && { 'Authorization': this.configuration.apiKey('Authorization') }), // Bearer authentication
        };

        return this.request<Array<MemberVM>>({
            url: '/api/Member/GetAllMembers',
            method: 'POST',
            headers,
        }, opts?.responseOpts);
    };

    /**
     */
    apiMemberGetMemberByUserIdPost({ id }: ApiMemberGetMemberByUserIdPostRequest): Observable<MemberVM>
    apiMemberGetMemberByUserIdPost({ id }: ApiMemberGetMemberByUserIdPostRequest, opts?: OperationOpts): Observable<AjaxResponse<MemberVM>>
    apiMemberGetMemberByUserIdPost({ id }: ApiMemberGetMemberByUserIdPostRequest, opts?: OperationOpts): Observable<MemberVM | AjaxResponse<MemberVM>> {

        const headers: HttpHeaders = {
            ...(this.configuration.apiKey && { 'Authorization': this.configuration.apiKey('Authorization') }), // Bearer authentication
        };

        const query: HttpQuery = {};

        if (id != null) { query['id'] = id; }

        return this.request<MemberVM>({
            url: '/api/Member/GetMemberByUserId',
            method: 'POST',
            headers,
            query,
        }, opts?.responseOpts);
    };

    /**
     */
    apiMemberUpdateMemberPost({ memberVM }: ApiMemberUpdateMemberPostRequest): Observable<void>
    apiMemberUpdateMemberPost({ memberVM }: ApiMemberUpdateMemberPostRequest, opts?: OperationOpts): Observable<void | AjaxResponse<void>>
    apiMemberUpdateMemberPost({ memberVM }: ApiMemberUpdateMemberPostRequest, opts?: OperationOpts): Observable<void | AjaxResponse<void>> {

        const headers: HttpHeaders = {
            'Content-Type': 'application/json',
            ...(this.configuration.apiKey && { 'Authorization': this.configuration.apiKey('Authorization') }), // Bearer authentication
        };

        return this.request<void>({
            url: '/api/Member/UpdateMember',
            method: 'POST',
            headers,
            body: memberVM,
        }, opts?.responseOpts);
    };

}
