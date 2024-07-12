import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "searchFilter",
  standalone: true
})
export class SearchFilterPipe implements PipeTransform {
  // Search by specified proprety
  transform(data: any[], filterProperty: string, filter: string): any[] {
    // prep search text
    const filterValue = filter.toLowerCase();
    // lookup and return
    return filterValue
      ? data.filter(item => item[filterProperty].toLowerCase().includes(filterValue))
      : data;
  }

}