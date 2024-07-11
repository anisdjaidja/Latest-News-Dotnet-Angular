import { Injectable } from '@angular/core';
import { Article } from '../Models/article.model';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  constructor() { }

  allArticles!: Article[]
}
