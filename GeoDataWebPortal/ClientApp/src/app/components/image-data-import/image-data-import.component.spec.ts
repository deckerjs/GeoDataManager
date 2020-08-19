import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageDataImportComponent } from './image-data-import.component';

describe('ImageDataImportComponent', () => {
  let component: ImageDataImportComponent;
  let fixture: ComponentFixture<ImageDataImportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageDataImportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageDataImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
