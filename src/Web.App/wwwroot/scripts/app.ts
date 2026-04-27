function changeTheme(theme: string): void {
  const app: Element | null = document.querySelector('#app');

  app?.setAttribute('data-theme', theme);
}

function close(dialog: HTMLDialogElement): void {
  dialog.close();
}

function show(dialog: HTMLDialogElement): void {
  dialog.showModal();
}

function scrollContentToTop(dialog: HTMLDialogElement, css: string): void {
  const content: Element | null = dialog.querySelector(css);
  content?.scrollTo({ top: 0, behavior: 'instant' });
}
