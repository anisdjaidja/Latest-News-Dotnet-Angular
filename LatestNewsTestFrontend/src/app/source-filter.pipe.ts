import { Pipe, PipeTransform } from '@angular/core';
import { Article } from './Models/article.model';


@Pipe({
  name: 'sourceFilter',
  standalone: true
})
export class SourceFilterPipe implements PipeTransform {
  // Filter by a list of sources
  transform(articles: Article[], terms: string[]): any {
    //check if the search term is defined
    if (articles.length <= 0 || terms.length <= 0) return articles;

    //return updated articles array
    return articles.filter(({ source }) => terms.includes(source.name.toLowerCase()));


  }

}