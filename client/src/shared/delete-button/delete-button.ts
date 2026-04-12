import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-delete-button',
  imports: [],
  templateUrl: './delete-button.html',
  styleUrl: './delete-button.css',
})
export class DeleteButton {
   disabled= input<boolean>();
   ClickEvent = output<Event>();
   onClick(event:Event){
    this.ClickEvent.emit(event);
   }
}
