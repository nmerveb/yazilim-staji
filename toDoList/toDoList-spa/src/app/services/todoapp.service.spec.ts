import { TestBed } from '@angular/core/testing';

import { TodoappService } from './todoapp.service';

describe('TodoappService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TodoappService = TestBed.get(TodoappService);
    expect(service).toBeTruthy();
  });
});
