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

/**
 * @export
 * @interface RegisterVM
 */
export interface RegisterVM {
    /**
     * @type {string}
     * @memberof RegisterVM
     */
    email: string;
    /**
     * @type {string}
     * @memberof RegisterVM
     */
    name: string;
    /**
     * @type {string}
     * @memberof RegisterVM
     */
    phone: string;
    /**
     * @type {string}
     * @memberof RegisterVM
     */
    pwd: string;
    /**
     * @type {string}
     * @memberof RegisterVM
     */
    recaptchaToken: string;
    /**
     * @type {boolean}
     * @memberof RegisterVM
     */
    rememberMe?: boolean;
}
