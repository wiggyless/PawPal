import { PageRequest } from './page-request';

// Mirrors the C# BasePagedQuery.cs class
export class BasePagedQuery {
  paging: PageRequest;

  constructor() {
    this.paging = new PageRequest();
  }
}
