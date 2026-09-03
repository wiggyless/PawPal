export interface PostAnswerDTO {
  answers: Record<number, string>;
}
export interface VerifyAnswerDTO {
  email: string;
  answers: Record<number, string>;
}
export interface IsAnswerTrue {
  isTrueAnswer: boolean;
}
