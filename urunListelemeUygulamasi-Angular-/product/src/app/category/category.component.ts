import { Component, OnInit } from '@angular/core';
import { Category } from './category';
import { CategoryService } from '../services/categories.service';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css'],
  providers: [CategoryService]
})
export class CategoryComponent implements OnInit {

  constructor(private categoryService: CategoryService,
    private alertifyService: AlertifyService) { }
  title = "Kategoriler"
  categories: Category[];
  ngOnInit() {

    this.categoryService.getCategories().subscribe(data=>{
      this.categories = data
    })
  }

}

