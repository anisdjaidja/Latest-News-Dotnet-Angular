import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ArticleService } from './Services/article.service';
import { Article } from './Models/article.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'LatestNewsTestFrontend';

  constructor(private httpclient: HttpClient, private articleService: ArticleService) { }

  ngOnInit(): void {
    this.httpclient.get<Article[]>('https://localhost:7036/GetAll')
      .subscribe(result => {
        this.articleService.allArticles = result;
        console.log(this.articleService.allArticles);
      })
  }
}
