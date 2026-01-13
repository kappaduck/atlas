export function changeTheme(theme: string): void {
  const app: Element | null = document.querySelector('#app');

  app?.setAttribute('data-theme', theme);
}
