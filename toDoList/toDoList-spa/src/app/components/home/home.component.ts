import { Component, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { TodoappService } from 'src/app/services/todoapp.service';
import { AuthService } from 'src/app/services/auth.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private todoappService: TodoappService,
    private authService: AuthService) {

  }

  data = {};
  get isAuthenticated(){
    return this.authService.loggedIn();
  }
  get userId(){
    return localStorage.getItem("userId");
  }
  logOut(){
    this.authService.logOut();
   }
  //Sürükle bırak işlemi bu metotta çalışır.
  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }
    this.updateLists(event);
  }


  ngOnInit() {
    this.getYourLists();
    }

  updateLists(event: CdkDragDrop<string[]>){
    const obj = event.container.data[event.currentIndex];
    var listname=event.container.id;
    var comefrom = event.previousContainer.id;
    this.todoappService.updateLists(listname,comefrom,obj)
    .subscribe((res) => {
      console.log(res);
      this.getYourLists();
    },
      (err) => { console.log(err); });
  }

  getYourLists(){
    var userid =+this.userId;
    this.todoappService.getYourLists(userid)
    .subscribe((res) => {
      console.log(res);
      Object.keys(res).forEach((key) => {
        this.data[key] = res[key];
      })
    }, (err) => { console.log(err); });
  }

  addPendings(pending){
    var userid=+ this.userId;
    const obj = { todo: pending.value, UserId: userid }
    this.todoappService.addPendings(obj)
    .subscribe((res:any) => {
      this.getYourLists();
      pending.value = '';
    }, (err) => { console.log(err); });
  }
  
 delete(id, listname){
  if(confirm('Bu maddeyi silmek istediğinize emin misiniz?'))
  {
    this.todoappService.delete(id,listname)
    .subscribe((res) => {
      console.log(res);
      this.getYourLists();

    }, (err) => {
      console.log(err);
    });
  }
  
}
}

  
