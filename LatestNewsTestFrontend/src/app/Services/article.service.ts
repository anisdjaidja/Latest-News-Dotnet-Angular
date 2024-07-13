import { Injectable } from '@angular/core';
import { Article, ArticlesResponse } from '../Models/article.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  constructor(private httpclient: HttpClient) { }
  allArticles!: Article[]

  public requestNews() {

    if (sessionStorage.getItem('ArticlesCache') != null) {
      console.log("Found chached articles")
      let cachedData: ArticlesResponse = JSON.parse(sessionStorage.getItem('ArticlesCache')!);
      let lastID = cachedData.latestID;
      console.log("Latest Stored ID : ", lastID)
      this.allArticles = cachedData.articles;
      try {
        this.httpclient.get<ArticlesResponse>('https://localhost:7036/News/getall' + '/' + lastID)
          .subscribe(result => {
            if (result != null) {
              let results = result.articles.sort((a, b) => new Date(b.publishedAt).getTime() - new Date(a.publishedAt).getTime());;
              cachedData.latestID = result.latestID;
              cachedData.articles = cachedData.articles.concat(results);
              this.allArticles = cachedData.articles;
              console.log(this.allArticles);
              sessionStorage.setItem('ArticlesCache', JSON.stringify(cachedData));
            }

          })
      } catch (error) {
        console.log("204 No updates found, loading from cache only")
      }

      return;
    }
    this.httpclient.get<ArticlesResponse>('https://localhost:7036/News/getall')
      .subscribe(result => {
        let results = result.articles.sort((a, b) => new Date(b.publishedAt).getTime() - new Date(a.publishedAt).getTime());;
        this.allArticles = results;
        console.log(this.allArticles);
        sessionStorage.setItem('ArticlesCache', JSON.stringify(result));
      })
  }

}
