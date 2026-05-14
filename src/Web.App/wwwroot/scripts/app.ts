function changeTheme(theme: string): void {
  const app: Element | null = document.querySelector('#app');

  app?.setAttribute('data-theme', theme);
}

function initSettings(dialog: HTMLDialogElement, dotnet: DotNet): void {
  dialog.addEventListener('command', (event) => {
    const ev: CommandEvent = event as CommandEvent;

    if (ev.command === '--general') {
      dotnet.invokeMethod('ShowGeneralSection');
    }

    if (ev.command === '--changelog') {
      dotnet.invokeMethod('ShowChangelogSection');
    }

    dialog.showModal();
  });
}
