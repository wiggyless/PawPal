// payload kako dolazi iz JWT-a
export interface JwtPayloadDto {
  sub: string;
  email: string;
  role_id:number;
  ver: string;
  iat: number;
  exp: number;
  aud: string;
  iss: string;
}
