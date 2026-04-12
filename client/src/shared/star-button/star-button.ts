import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-star-button',
  imports: [],
  templateUrl: './star-button.html',
  styleUrl: './star-button.css',
})
export class StarButton {
   disabled= input<boolean>();
   selected = input<boolean>();
   ClickEvent = output<Event>();
   onClick(event:Event){
    this.ClickEvent.emit(event);
   }
}
