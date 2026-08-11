type DotNet = {
  invokeMethod: (method: string) => void;
}

let eventCallback: (this: Document, ev: MouseEvent) => any;

export function scrollToCountry(id: string) {
  const element: HTMLElement | null = document.getElementById(id);

  if (!element) {
    return;
  }

  element.scrollIntoView({ behavior: 'instant', block: 'nearest' });
}

export function init(dotnet: DotNet) {
  eventCallback = (event: MouseEvent) => {
    const lookup: Element | null = document.querySelector('.quack-combobox');

    if (!event || !event.target || !(event.target instanceof Element)) {
      return;
    }

    if (event.target.classList.contains('quack-combobox-option')) {
      return;
    }

    if (lookup && !lookup.contains(event.target)) {
      dotnet.invokeMethod('Clear');
    }
  };

  document.addEventListener('click', eventCallback);
}

export function dispose() {
  document.removeEventListener('click', eventCallback);
}
