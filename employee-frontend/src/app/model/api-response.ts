export interface ApiResponse<T> {
  code: number;       // HTTP status code
  message: string;    // Response message (e.g., "Success", "Error: Invalid request")
  data: T;            // Generic data (can be object, tuple, etc.)
}
