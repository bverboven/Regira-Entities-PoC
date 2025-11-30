<template>
    <div v-if="$isDebug" class="debug text-white bg-secondary">
        <div class="row g-0 gx-1">
            <div class="col-auto py-1">Screen: {{ $screen.size }}</div>
            <div class="col text-center py-1">{{ $appStatus }}: {{ $router.currentRoute.value.name }} {{ $router.currentRoute.value.params }}</div>
            <div class="col-auto py-1">{{ $culture }}</div>
            <div class="col-auto text-uppercase py-1">{{ $screen.layout }}</div>
            <div class="col-auto">
                <IconButton icon="close" class="p-1" @click="$setDebug(false)" />
            </div>
        </div>
    </div>
</template>
