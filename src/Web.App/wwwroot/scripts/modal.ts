export function showModal(dialog: HTMLDialogElement): void {
  dialog.showModal();
}

export function closeModal(dialog: HTMLDialogElement): void {
  dialog.close();
}

export function closeModalOnClickOutside(dialog: HTMLDialogElement): void {
  dialog.addEventListener('click', event => {
    if (event.target === dialog) {
      closeModal(dialog);
    }
  });
}

export function scrollContentToTop(dialog: HTMLDialogElement): void {
  const content = dialog.querySelector('.content');

  content?.scrollTo({ top: 0, behavior: 'instant' });
}
