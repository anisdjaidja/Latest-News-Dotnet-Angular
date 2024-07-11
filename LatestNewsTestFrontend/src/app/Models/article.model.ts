import { Source } from "./source.model";

export interface Article {
  id: number,
  title: string,
  autor: string,
  description: string,
  url: string,
  urlToImage: string,
  publishedAt: Date,
  content: string,

  source: Source,
}
