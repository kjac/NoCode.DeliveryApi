export interface ClientDetails extends ClientBase {
  id?: string;
}

export interface ClientBase {
  name: string;
  origin: string;
  previewUrlPath?: string | null;
  publishedUrlPath?: string | null;
  culture?: string | null;
}
