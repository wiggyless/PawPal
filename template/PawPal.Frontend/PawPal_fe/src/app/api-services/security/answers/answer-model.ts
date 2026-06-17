export interface GetAndPostAnswerDTO {
  email: string;
  answers: Record<number, string>;
}
export interface IsAnswerTrue {
  isTrueAnswer: boolean;
}
