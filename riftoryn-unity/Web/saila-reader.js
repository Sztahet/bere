(() => {
  "use strict";
  const storyFolder = document.currentScript.dataset.story === "awakening" ? "SailaAwakening" : "SailaStory";
  const base = new URL(`../Assets/Resources/${storyFolder}/`, document.currentScript.src);
  const nav = document.getElementById("story-nav");
  const container = document.getElementById("story-chapters");
  const make = (tag, text, className) => {
    const node = document.createElement(tag);
    if (text != null) node.textContent = text;
    if (className) node.className = className;
    return node;
  };
  fetch(new URL("story.json", base))
    .then(response => { if (!response.ok) throw new Error(`HTTP ${response.status}`); return response.json(); })
    .then(story => {
      if (!Array.isArray(story.chapters) || !story.chapters.length) throw new Error("Brak rozdziałów");
      const sections = document.createDocumentFragment();
      const links = document.createDocumentFragment();
      story.chapters.forEach((chapter, index) => {
        const number = String(index + 1).padStart(2, "0");
        const title = chapter.title.replace(/^\d+\.\s*/, "");
        const section = make("section", null, "chapter");
        section.id = `rozdzial-${index + 1}`;
        const heading = make("div", null, "chapter-heading");
        heading.append(make("span", `ROZDZIAŁ ${number}`), make("h2", title));
        const prose = make("div", null, "prose");
        chapter.blocks.forEach(block => {
          if (block.image > 0) {
            const figure = make("figure");
            const image = make("img");
            image.src = new URL(`${String(block.image).padStart(2, "0")}.jpg`, base).href;
            image.alt = block.alt || "Ilustracja do opowieści o Saili.";
            image.loading = "lazy";
            image.width = 1536; image.height = 1024;
            figure.append(image); prose.append(figure);
          } else if (typeof block.text === "string") prose.append(make("p", block.text));
        });
        section.append(heading, prose); sections.append(section);
        const link = make("a"); link.href = `#${section.id}`;
        link.append(make("span", number), document.createTextNode(` ${title}`)); links.append(link);
      });
      container.replaceChildren(sections); nav.replaceChildren(links);
      if (/^#rozdzial-\d+$/.test(location.hash)) document.getElementById(location.hash.slice(1))?.scrollIntoView();
    })
    .catch(() => { container.replaceChildren(make("p", "Nie udało się wczytać opowieści. Odśwież stronę lub spróbuj ponownie później.")); });
})();
