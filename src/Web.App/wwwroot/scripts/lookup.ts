type DotNet = {
  invokeMethod: (method: string) => void;
}

let eventCallback: (this: Document, ev: MouseEvent) => any;

export function scrollToLookup() {
  const element = document.querySelector('.container');

  if (!element) {
    return;
  }

  setTimeout(() => {
    element.scrollIntoView({ behavior: 'smooth', block: 'center' });
  }, 300);
}

export function scrollToCountry(id: string) {
  const element = document.getElementById(id);

  if (!element) {
    return;
  }

  element.scrollIntoView({ behavior: 'instant', block: 'nearest' });
}

export function init(dotnet: DotNet) {
  eventCallback = (event: MouseEvent) => {
    const lookup = document.querySelector('.container');

    if (!event || !event.target || !(event.target instanceof Element)) {
      return;
    }

    if (event.target.classList.contains('item')) {
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
