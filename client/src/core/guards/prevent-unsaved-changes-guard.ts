import { CanDeactivateFn } from '@angular/router';
import { MemberProfile } from '../../features/members/member-profile/member-profile';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberProfile> = (component) => {
  if(component.editForm?.dirty){
    return confirm('Dude,Are you sure you want to exit? All unsaved changes will be lost');

  }
  return true;
};
