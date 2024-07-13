import { HttpClient } from '@angular/common/http';
import { Component, inject, signal, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { LiveAnnouncer } from '@angular/cdk/a11y';
// Local imports
import { ArticleService } from './Services/article.service';
import { SearchFilterPipe } from './search-filter.pipe';
import { SourceFilterPipe } from './source-filter.pipe';
/// UI imports
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatChipEditedEvent, MatChipInputEvent, MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,
    MatCardModule,
    MatGridListModule,
    MatButtonModule,
    CommonModule,
    NgbPaginationModule,
    MatFormFieldModule,
    MatChipsModule,
    MatIconModule,
    SearchFilterPipe,
    MatInputModule,
    SourceFilterPipe,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  readonly announcer = inject(LiveAnnouncer);
  readonly title = 'Latest News Test Frontend';
  page = 1;
  pageSize = 50;
  searchText: string = "";
  readonly limitedSources = signal<string[]>([]);
  readonly separatorKeysCodes = [ENTER, COMMA] as const;

  constructor(private httpclient: HttpClient,
    public articleService: ArticleService,) { }

  ngOnInit(): void {
    this.articleService.requestNews();
  }
  protected onInput(event: Event) {
    this.searchText = (event.target as HTMLInputElement).value;
  }
  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (value) {
      this.limitedSources.update(limitedSources => [...limitedSources, value]);
    }
    // Clear the input value
    event.chipInput!.clear();
  }
  remove(word: string): void {
    this.limitedSources.update(limitedSources => {
      const index = limitedSources.indexOf(word);
      if (index < 0) {
        return limitedSources;
      }

      limitedSources.splice(index, 1);
      this.announcer.announce(`Removed ${word}`);
      return [...limitedSources];
    });
  }
  edit(word: string, event: MatChipEditedEvent) {
    const value = event.value.trim();

    // Remove source if it no longer has a name
    if (!value) {
      this.remove(word);
      return;
    }

    // Edit existing filters
    this.limitedSources.update(limitedSources => {
      const index = limitedSources.indexOf(word);
      if (index >= 0) {
        limitedSources[index] = value;
        return [...limitedSources];
      }
      return limitedSources;
    });
  }
}
