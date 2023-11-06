<template>
  <div class="box-wrap">
    <div class="title">
      <slot name="title"></slot>
    </div>

    <div class="slides">
      <button @click="prevSlide">‹</button>

      <div
        v-for="(slide, i) in extendedSlides"
        class="slide"
        :class="{ 'slide-active': slide.offset === 0 }"
        :key="i"
        :style="{
          '--offset': slide.offset,
          '--dir': slide.offset === 0 ? 0 : slide.offset > 0 ? 1 : -1,
        }"
        :data-active="slide.offset === 0 ? true : null"
        @mousemove="handleMouseMove"
        ref="slidesRef"
      >
        <div
          className="slideContent"
          :style="{ backgroundImage: `url('${slide.image}')` }"
        >
          <div className="slideContentInner">
            <h2 className="slideTitle">{{ slide.title }}</h2>
            <h3 className="slideSubtitle">{{ slide.subtitle }}</h3>
            <p className="slideDescription">{{ slide.description }}</p>
          </div>
        </div>
      </div>

      <button @click="nextSlide">›</button>
    </div>

    <div class="bg-wrap">
      <img
        className="slideBackground"
        :src="extendedSlides[slideIndex].image"
      />
    </div>
  </div>
</template>

<script lang="ts">
import { ref, computed } from 'vue';

export default {
  name: 'SlideShow',
  setup() {
    const slideIndex = ref(0);
    const slides = [
      {
        title: 'Machu Picchu',
        subtitle: 'Peru',
        description: 'Adventure is never far away',
        image:
          'https://images.unsplash.com/photo-1571771019784-3ff35f4f4277?ixlib=rb-1.2.1&q=80&fm=jpg&crop=entropy&cs=tinysrgb&w=800&fit=max&ixid=eyJhcHBfaWQiOjE0NTg5fQ',
      },
      {
        title: 'Chamonix',
        subtitle: 'France',
        description: 'Let your dreams come true',
        image:
          'https://images.unsplash.com/photo-1581836499506-4a660b39478a?ixlib=rb-1.2.1&q=80&fm=jpg&crop=entropy&cs=tinysrgb&w=800&fit=max&ixid=eyJhcHBfaWQiOjE0NTg5fQ',
      },
      {
        title: 'Mimisa Rocks',
        subtitle: 'Australia',
        description: 'A piece of heaven',
        image:
          'https://images.unsplash.com/photo-1566522650166-bd8b3e3a2b4b?ixlib=rb-1.2.1&q=80&fm=jpg&crop=entropy&cs=tinysrgb&w=800&fit=max&ixid=eyJhcHBfaWQiOjE0NTg5fQ',
      },
      {
        title: 'Four',
        subtitle: 'Australia',
        description: 'A piece of heaven',
        image:
          'https://images.unsplash.com/flagged/photo-1564918031455-72f4e35ba7a6?ixlib=rb-1.2.1&q=80&fm=jpg&crop=entropy&cs=tinysrgb&w=800&fit=max&ixid=eyJhcHBfaWQiOjE0NTg5fQ',
      },
      {
        title: 'Five',
        subtitle: 'Australia',
        description: 'A piece of heaven',
        image:
          'https://images.unsplash.com/photo-1579130781921-76e18892b57b?ixlib=rb-1.2.1&q=80&fm=jpg&crop=entropy&cs=tinysrgb&w=800&fit=max&ixid=eyJhcHBfaWQiOjE0NTg5fQ',
      },
    ];

    const prevSlide = () => {
      slideIndex.value = (slideIndex.value - 1 + slides.length) % slides.length;
    };

    const nextSlide = () => {
      slideIndex.value = (slideIndex.value + 1) % slides.length;
    };

    const extendedSlides = computed(() => {
      return [...slides, ...slides, ...slides].map((slide, i) => ({
        ...slide,
        offset: slides.length + (slideIndex.value - i),
      }));
    });

    const slidesRef: any = ref([]); // 创建一个 ref 来存储幻灯片元素的引用

    // 在页面加载后，将幻灯片元素的引用存储到 slidesRef 中
    const handleRefs = (el: Element, index: number) => {
      slidesRef.value[index] = el;
    };

    // 在鼠标移动时，根据鼠标位置更新幻灯片的样式
    const handleMouseMove = (e: MouseEvent) => {
      const el: any = e.target;

      if (el) {
        const mouseX = e.clientX;
        const mouseY = e.clientY;
        const rect = el.getBoundingClientRect();
        const px = (mouseX - rect.left) / rect.width;
        const py = (mouseY - rect.top) / rect.height;
        el.style.setProperty('--px', String(px));
        el.style.setProperty('--py', String(py));
      }
    };

    return {
      slideIndex,
      slides,
      prevSlide,
      nextSlide,
      extendedSlides,
      handleRefs,
      handleMouseMove,
      slidesRef,
    };
  },
};
</script>

<style lang="scss" scoped>
.box-wrap {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 400px;
  width: 100%;
  margin: 0;
  padding: 0;
  font-size: 3vmin;
  color: #fff;
  overflow: hidden;
  position: relative;
}

*,
*::before,
*::after {
  box-sizing: border-box;
  position: relative;
}

.slides {
  display: grid;
  > .slide {
    grid-area: 1 / -1;
    cursor: pointer;
  }

  > button {
    appearance: none;
    background: transparent;
    border: none;
    color: white;
    position: absolute;
    font-size: 5rem;
    width: 5rem;
    height: 5rem;
    top: 30%;
    transition: opacity 0.3s;
    opacity: 0.7;
    z-index: 5;

    &:hover {
      opacity: 1;
    }

    &:focus {
      outline: none;
    }

    &:first-child {
      left: -50%;
    }
    &:last-child {
      right: -50%;
    }
  }
}

.slideContent {
  height: 200px;
  background-size: cover;
  background-position: center center;
  background-repeat: no-repeat;
  transition: transform 0.5s ease-in-out;
  opacity: 0.7;
  border-radius: 30px;

  display: grid;
  align-content: center;

  transform-style: preserve-3d;
  transform: perspective(1000px) translateX(calc(100% * var(--offset)))
    rotateY(calc(-45deg * var(--dir)));
}

.slideContentInner {
  transform-style: preserve-3d;
  transform: translateZ(2rem);
  transition: opacity 0.3s linear;
  text-shadow: 0 0.1rem 1rem #000;
  opacity: 0;

  .slideSubtitle,
  .slideTitle {
    font-size: 2rem;
    font-weight: normal;
    letter-spacing: 0.2ch;
    text-transform: uppercase;
    margin: 0;
  }

  .slideSubtitle::before {
    content: '— ';
  }

  .slideDescription {
    margin: 0;
    font-size: 0.8rem;
    letter-spacing: 0.2ch;
  }
}

.slideBackground {
  height: 100%;
  width: 100%;
  position: relative;
  top: 0;
  left: 0;
  background-size: cover;
  background-position: center center;
  opacity: 0.2;
  z-index: -1;
  transition: opacity 0.3s linear, transform 0.3s ease-in-out;
  pointer-events: none;
}

.slide[data-active] {
  z-index: 2;
  pointer-events: auto;

  .slideBackground {
    opacity: 0.8;
    transform: none;
  }

  .slideContentInner {
    opacity: 1;
  }

  .slideContent {
    --x: calc(var(--px) - 0.5);
    --y: calc(var(--py) - 0.5);
    opacity: 1;

    transform: perspective(1000px);

    &:hover {
      transition: none;
      transform: perspective(1000px) rotateY(calc(var(--x) * 45deg))
        rotateX(calc(var(--y) * -45deg));
    }
    @include box-shadow;
  }
}

.bg-wrap {
  position: absolute;
  top: 0;
  width: 100%;
  height: 600px;
  overflow: hidden;
  background: #000;
  opacity: 0.8;
}

.title {
  z-index: 1;
  margin-bottom: 50px;
}
</style>
