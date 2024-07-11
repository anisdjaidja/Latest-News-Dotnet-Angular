import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ArticleService } from './Services/article.service';
import { Article } from './Models/article.model';
import { HttpClientModule } from "@angular/common/http";
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,
    MatCardModule,
    MatGridListModule,
    MatButtonModule,
    CommonModule,
    NgbPaginationModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Latest News Test Frontend';
  page = 1;
  pageSize = 50;
  constructor(private httpclient: HttpClient, public articleService: ArticleService) { }

  ngOnInit(): void {
    this.httpclient.get<Article[]>('https://localhost:7036/News/GetAll')
      .subscribe(result => {
        this.articleService.allArticles = result;
        console.log(this.articleService.allArticles);
      })
  }
}
