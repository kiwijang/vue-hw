import { catchError } from 'rxjs/operators'
import { type IReCaptchaComposition } from 'vue-recaptcha-v3'
import {
  AuthApi,
  Configuration,
  type ConfigurationParameters,
  EventApi,
  MemberApi,
  UserApi
} from './api'
import jwtDecode from 'jwt-decode'
import { useLoginStore } from './stores/isLogin'

export const recaptchaExec = async (
  action: string,
  recaptchaInstance: IReCaptchaComposition | undefined
) => {
  // 等待載入完成
  await recaptchaInstance?.recaptchaLoaded()
  const token = await recaptchaInstance?.executeRecaptcha(action)
  console.log(token)
  return token
}

export const setBrowserStorage = (key: string, data: string) => {
  // 存儲到 Session Storage
  sessionStorage.setItem(key, data)
  // 存儲到 Local Storage
  localStorage.setItem(key, data)
}

export const getBrowserStorage = (key: string) => {
  // 從 Session Storage 中讀取數據
  const sessionData = sessionStorage.getItem(key)
  // 從 Local Storage 中讀取數據
  const localData = localStorage.getItem(key)

  return [sessionData, localData]
}

export const removeBrowserStorage = (key: string) => {
  // 從 Session Storage 中讀取數據
  const sessionData = sessionStorage.removeItem(key)
  // 從 Local Storage 中讀取數據
  const localData = localStorage.removeItem(key)

  return [sessionData, localData]
}

export const checkJwtIsLogin = () => {
  const [session, local] = getBrowserStorage('jwt')
  if (session && local) {
    return true
  } else {
    return false
  }
}

export const getJWTfromBrowser = () => {
  const [session, local] = getBrowserStorage('jwt')
  if (session && local) {
    return session
  } else {
    return null
  }
}

export const checkRoleEqual = (validRole: string) => {
  const encodedJwt = getJWTfromBrowser()
  let role = ''
  if (encodedJwt) {
    const jwt: any = jwtDecode(encodedJwt)
    role = jwt?.role
  }

  if (validRole === role) {
    return true
  } else {
    return false
  }
}

// api 設定
const configParameters: ConfigurationParameters = {
  // headers: {
  //   Authorization: sessionToken ? `Bearer ${sessionToken}` : "",
  // },
  // basePath: 'http://localhost:5160'
  basePath: 'https://localhost:7262'
}

// this.configuration.apiKey('Authorization')
const configParameters_JWT = (jwt: string) => {
  return {
    apiKey: 'Bearer ' + jwt,
    // basePath: 'http://localhost:5160'
    basePath: 'https://localhost:7262',
    middleware: [{ post: interceptFetchRes }]
  } as unknown as ConfigurationParameters
}

export const interceptFetchRes = (context: any) => {
  const { response, status } = context

  console.log(status)
  if (status === 401) {
    invailidJwtRelaodPage(401)
  } else {
    return context
  }
}

export const invailidJwtRelaodPage = (status: number) => {
  if (status === 401) {
    // 錯誤處理
    alert('token 30 分鐘過期!')
    const s = auth_api.apiAuthLogoutPost().subscribe()

    removeBrowserStorage('jwt')
    s.unsubscribe()

    // 更新 isLogin 變數
    const loginStore = useLoginStore()
    loginStore.checkIsLogin()

    // 重整網頁
    location.reload()
  }
}

export const configuration = new Configuration(configParameters)
export const configurationJWT = (jwt: string) =>
  new Configuration(configParameters_JWT(jwt) as unknown as ConfigurationParameters)

/// API
export const auth_api = new AuthApi(configuration)
export const event_api = (encodedJwt: string) => new EventApi(configurationJWT(encodedJwt ?? ''))
export const member_api = (encodedJwt: string) => new MemberApi(configurationJWT(encodedJwt ?? ''))
export const user_api = (encodedJwt: string) => new UserApi(configurationJWT(encodedJwt ?? ''))
