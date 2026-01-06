export const NET_CLAIM_TYPES = {
    NameIdentifier: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
    Email: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress',
} as const;

// payload kako dolazi iz JWT-a
export interface JwtPayloadDto {
  sub: string;
  [NET_CLAIM_TYPES.NameIdentifier]: string;
  [NET_CLAIM_TYPES.Email]: string;
  email: string;
  role_id:number;
  ver: string;
  iat: number;
  exp: number;
  aud: string;
  iss: string;
}
