export interface ApiResponse<T> {
  data: T | null;
  error: string | null;
}
