import { HttpParams } from '@angular/common/http';

export function buildHttpParams(obj: Record<string, any>, prefix: string = ''): HttpParams {
  let params = new HttpParams();

  if (obj === undefined || obj === null) {
    return params;
  }
  Object.entries(obj).forEach(([key, value]) => {
    const fullKey = prefix ? `${prefix}.${key}` : key;
    if (value === null || value === undefined) {
      return;
    }

    if (typeof value === 'string' && value.trim() === '') {
      return;
    }

    if (Array.isArray(value)) {
      value.forEach((val) => {
        if (val !== null && val !== undefined) {
          params = params.append(fullKey, String(val));
        }
      });
      return;
    }

    if (typeof value === 'object' && !(value instanceof Date)) {
      const nestedParams = buildHttpParams(value, fullKey);
      nestedParams.keys().forEach((nestedKey) => {
        nestedParams.getAll(nestedKey)?.forEach((nestedValue) => {
          params = params.append(nestedKey, nestedValue);
        });
      });
      return;
    }

    if (value instanceof Date) {
      params = params.set(fullKey, value.toISOString());
      return;
    }

    params = params.set(fullKey, String(value));
  });

  return params;
}
